using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Business.Contracts.Services.CycleEditor;
using Business.Contracts.ViewModels.Common;
using Business.Contracts.ViewModels.CycleEditor;
using Business.Contracts.ViewModels.GeneEditor;
using Data.Contracts;
using Data.Contracts.Entities.CycleEditor;
using Data.Contracts.Entities.Identity;
using Shared.Enum;
using Shared.Framework.Dependency;
using Shared.Framework.Security;

namespace Services.Services.CycleEditor
{
    public class CycleEditorService : ICycleEditorService, IDependency
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IIdentityStorage identityStorage;

        public CycleEditorService(IUnitOfWork unitOfWork, IIdentityStorage identityStorage)
        {
            this.unitOfWork = unitOfWork;
            this.identityStorage = identityStorage;
        }

        public async Task<IEnumerable<SynthesisCycleItemViewModel>> GetSynthesisCycles()
        {
            var currentPrincipal = identityStorage.GetPrincipal();
            IEnumerable<SynthesisCycle> allCycles = await unitOfWork.GetAll<SynthesisCycle>()
                .Include(x => x.CycleSteps)
                .Where(x => x.UserId == currentPrincipal.UserId)
                .ToListAsync();

            IList<SynthesisCycleItemViewModel> allCycleItems = new List<SynthesisCycleItemViewModel>();
            foreach (SynthesisCycle synthesisCycle in allCycles.OrderByDescending(x => x.CreatedAt))
            {
                int totalSteps = synthesisCycle.CycleSteps.Count();
                int totalTime = synthesisCycle.CycleSteps.Sum(x => x.StepTime);
                allCycleItems.Add(new SynthesisCycleItemViewModel() { Id = synthesisCycle.Id, Name = synthesisCycle.Name, TotalSteps = totalSteps, TotalTime = totalTime });
            }

            return allCycleItems;
        }

        public async Task DeleteSynthesisCycle(string synthesisCycleId)
        {
            await unitOfWork.DeleteWhere<SynthesisCycle>(x => x.Id == synthesisCycleId);
        }

        public async Task DeleteCycleStep(string cycleStepId)
        {
            CycleStep cycleStepToDelete = unitOfWork.GetById<CycleStep>(cycleStepId);
            await unitOfWork.DeleteWhere<CycleStep>(x => x.Id == cycleStepId);
            await UpdateStepNumbersOnDelete(cycleStepToDelete);
        }

        public async Task CreateOrUpdateSynthesisCycle(SynthesisCycleItemViewModel synthesisCycleItemViewModel)
        {
            SynthesisCycle synthesisCycle;

            if (string.IsNullOrWhiteSpace(synthesisCycleItemViewModel.Id))
            {
                var currentPrincipal = identityStorage.GetPrincipal();
                User currentUser = unitOfWork.GetById<User>(currentPrincipal.UserId);

                synthesisCycle = new SynthesisCycle();
                synthesisCycle.Name = synthesisCycleItemViewModel.Name;
                synthesisCycle.User = currentUser;

                SynthesisCycle defaultSynthesisCycle = await unitOfWork.GetAll<SynthesisCycle>()
                    .Include(x => x.CycleSteps)
                    .Include(x => x.CycleSteps.Select(y => y.HardwareFunction))
                    .SingleAsync(x => x.UserId == null);

                foreach (CycleStep defaultCycleStep in defaultSynthesisCycle.CycleSteps)
                {
                    CycleStep newCycleStep = new CycleStep();
                    newCycleStep.Number = defaultCycleStep.Number;
                    newCycleStep.StepTime = defaultCycleStep.StepTime;
                    newCycleStep.A = defaultCycleStep.A;
                    newCycleStep.G = defaultCycleStep.G;
                    newCycleStep.C = defaultCycleStep.C;
                    newCycleStep.T = defaultCycleStep.T;
                    newCycleStep.Six = defaultCycleStep.Six;
                    newCycleStep.Seven = defaultCycleStep.Seven;
                    newCycleStep.SafeStep = defaultCycleStep.SafeStep;
                    newCycleStep.HardwareFunction = defaultCycleStep.HardwareFunction;

                    synthesisCycle.CycleSteps.Add(newCycleStep);
                }
            }
            else
            {
                synthesisCycle = await unitOfWork.GetAll<SynthesisCycle>().Include(x => x.User).SingleAsync(x => x.Id == synthesisCycleItemViewModel.Id);
                synthesisCycle.Name = synthesisCycleItemViewModel.Name;
            }

            unitOfWork.InsertOrUpdate(synthesisCycle);
            unitOfWork.Commit();
        }
        
        public async Task CreateOrUpdateCycleStep(CycleStepItemViewModel cycleStepItemViewModel)
        {
            CycleStep cycleStepToInsertOrUpdate;

            if (cycleStepItemViewModel.IsNew)
            {
                cycleStepToInsertOrUpdate = new CycleStep();

                SynthesisCycle synthesisCycle = unitOfWork.GetById<SynthesisCycle>(cycleStepItemViewModel.SynthesisCycleId);
                cycleStepToInsertOrUpdate.SynthesisCycle = synthesisCycle;

                await MapCycleStepItemToCycleStep(cycleStepItemViewModel, cycleStepToInsertOrUpdate);
            }
            else
            {
                cycleStepToInsertOrUpdate = await unitOfWork.GetAll<CycleStep>()
                    .Include(x => x.SynthesisCycle)
                    .Include(x => x.HardwareFunction)
                    .SingleAsync(x => x.Id == cycleStepItemViewModel.CycleStepId);

                await MapCycleStepItemToCycleStep(cycleStepItemViewModel, cycleStepToInsertOrUpdate);
            }

            unitOfWork.InsertOrUpdate(cycleStepToInsertOrUpdate);
            unitOfWork.Commit();

            if (cycleStepItemViewModel.IsNew)
            {
                await UpdateStepNumbersOnInsert(cycleStepItemViewModel, cycleStepToInsertOrUpdate);
            }
        }

        private async Task UpdateStepNumbersOnInsert(CycleStepItemViewModel cycleStepItemViewModel, CycleStep cycleStepToInsertOrUpdate)
        {
            bool cycleStepWithTheSameNumberExists = await unitOfWork.GetAll<CycleStep>()
                .CountAsync(x => x.SynthesisCycleId == cycleStepItemViewModel.SynthesisCycleId && x.Number == cycleStepToInsertOrUpdate.Number) > 1;

            if (cycleStepWithTheSameNumberExists)
            {
                IEnumerable<CycleStep> cycleSteps = await unitOfWork.GetAll<CycleStep>()
                    .Include(x => x.SynthesisCycle)
                    .Include(x => x.HardwareFunction)
                    .Where(x => x.SynthesisCycleId == cycleStepItemViewModel.SynthesisCycleId &&
                                x.Number >= cycleStepToInsertOrUpdate.Number && x.Id != cycleStepToInsertOrUpdate.Id)
                    .OrderBy(x => x.Number)
                    .ToListAsync();

                foreach (CycleStep cycleStep in cycleSteps)
                {
                    cycleStep.Number++;
                }

                unitOfWork.Commit();
            }
        }

        private async Task UpdateStepNumbersOnDelete(CycleStep cycleStepToDelete)
        {
            IEnumerable<CycleStep> cycleSteps = await unitOfWork.GetAll<CycleStep>()
                .Include(x => x.SynthesisCycle)
                .Include(x => x.HardwareFunction)
                .Where(x => x.SynthesisCycleId == cycleStepToDelete.SynthesisCycleId &&
                            x.Number > cycleStepToDelete.Number)
                .OrderBy(x => x.Number)
                .ToListAsync();

            foreach (CycleStep cycleStep in cycleSteps)
            {
                cycleStep.Number--;
            }

            unitOfWork.Commit();

        }

        public async Task<IEnumerable<CycleStepItemViewModel>> GetCycleStepItems(string synthesisCycleId)
        {
            SynthesisCycle synthesisCycle = await unitOfWork.GetAll<SynthesisCycle>()
                    .Include(x => x.CycleSteps.Select(y => y.HardwareFunction))
                    .AsNoTracking()
                    .SingleAsync(x => x.Id == synthesisCycleId);

            IList<CycleStepItemViewModel> cycleStepItems = new List<CycleStepItemViewModel>();
            foreach (var cycleStep in synthesisCycle.CycleSteps.OrderBy(x => x.Number))
            {
                cycleStepItems.Add(new CycleStepItemViewModel()
                {
                    SynthesisCycleId = synthesisCycleId,
                    CycleStepId = cycleStep.Id,
                    HardwareFunctionId = cycleStep.HardwareFunctionId,
                    StepNumber = cycleStep.Number,
                    FunctionNumber = cycleStep.HardwareFunction.Number,
                    //FunctionName = cycleStep.HardwareFunction.Name,
                    HardwareFunction = new HardwareFunctionItemViewModel { Id = cycleStep.HardwareFunction.Id, Name = cycleStep.HardwareFunction.Name },
                    StepTime = cycleStep.StepTime,
                    A = cycleStep.A,
                    G = cycleStep.G,
                    C = cycleStep.C,
                    T = cycleStep.T,
                    Five = cycleStep.Five,
                    Six = cycleStep.Six,
                    Seven = cycleStep.Seven,
                    SafeStep = cycleStep.SafeStep,
                });
            }

            return cycleStepItems;
        }

        private async Task MapCycleStepItemToCycleStep(CycleStepItemViewModel cycleStepItemViewModel, CycleStep cycleStep)
        {
            cycleStep.Number = cycleStepItemViewModel.StepNumber != 0 ? cycleStepItemViewModel.StepNumber : 1;
            cycleStep.StepTime = cycleStepItemViewModel.StepTime;
            cycleStep.A = cycleStepItemViewModel.A;
            cycleStep.G = cycleStepItemViewModel.G;
            cycleStep.C = cycleStepItemViewModel.C;
            cycleStep.T = cycleStepItemViewModel.T;
            cycleStep.T = cycleStepItemViewModel.T;
            cycleStep.Six = cycleStepItemViewModel.Six;
            cycleStep.Seven = cycleStepItemViewModel.Seven;
            cycleStep.SafeStep = cycleStepItemViewModel.SafeStep;

            if (string.IsNullOrWhiteSpace(cycleStepItemViewModel.HardwareFunction?.Id))
            {
                cycleStep.HardwareFunction = await unitOfWork.GetAll<HardwareFunction>()
                    .SingleAsync(x => x.FunctionType == HardwareFunctionType.BAndTetToCol);
            }
            else if (cycleStep.HardwareFunctionId != cycleStepItemViewModel.HardwareFunction.Id)
            {
                cycleStep.HardwareFunction = unitOfWork.GetById<HardwareFunction>(cycleStepItemViewModel.HardwareFunction.Id);
            }
        }
    }
}