using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Business.Contracts.Services.ManualControl;
using Business.Contracts.ViewModels.Common;
using Business.Contracts.ViewModels.ManualControl;
using Data.Contracts;
using Data.Contracts.Entities.CycleEditor;
using Services.Services.Common;
using Shared.Enum;
using Shared.Framework.Collections;
using Shared.Framework.Dependency;
using Shared.Framework.Security;

namespace Services.Services.ManualControl
{
    public class ManualControlService : BaseService, IManualControlService, IDependency
    {
        public ManualControlService(IUnitOfWork unitOfWork, IIdentityStorage identityStorage)
            : base(unitOfWork, identityStorage)
        {
        }

        public async Task<ManualControlViewModel> GetManualControlViewModel()
        {
            ManualControlViewModel manualControlViewModel = new ManualControlViewModel();

            IEnumerable<HardwareFunction> activatedValves = 
                await unitOfWork.GetAll<HardwareFunction>().Where(x => x.FunctionType == HardwareFunctionType.Valve && x.IsActivated).ToListAsync();
            manualControlViewModel.IsValvesActivated = activatedValves != null && activatedValves.Any();

            IEnumerable<HardwareFunction> allSyringePumpFunctions = await unitOfWork.GetAll<HardwareFunction>()
                .Where(x => x.FunctionType == HardwareFunctionType.SyringePumpInit || 
                    x.FunctionType == HardwareFunctionType.SyringePumpFin || x.FunctionType == HardwareFunctionType.SyringePumpDraw)
                .ToListAsync();

            manualControlViewModel.AllSyringePumpHardwareFunctions = 
                allSyringePumpFunctions.Select(x => new ListItem() { Value = (int)x.FunctionType, Text = x.Name  });
            manualControlViewModel.SelectedSyringePumpFunction = HardwareFunctionType.SyringePumpInit;

            IEnumerable<HardwareFunction> allGSValves = await unitOfWork.GetAll<HardwareFunction>()
                .Where(x => x.FunctionType == HardwareFunctionType.GSValve)
                .ToListAsync();

            manualControlViewModel.AllGSValvesHardwareFunctions =
                allGSValves.Select(x => new ListItem() { Id = x.ApiUrl, Text = x.Name });
            manualControlViewModel.SelectedGSValveFunction = allGSValves.First().ApiUrl;

            return manualControlViewModel;
        }

        public void ActivateValves(IEnumerable<HardwareFunctionItemViewModel> selectedValves)
        {
            foreach (HardwareFunctionItemViewModel valve in selectedValves)
            {
                var valveHardwareFunction = unitOfWork.GetById<HardwareFunction>(valve.Id);
                valveHardwareFunction.IsActivated = true;
            }

            unitOfWork.Commit();
        }

        public async Task DeactivateAllValves()
        {
            IEnumerable<HardwareFunction> allActivatedValves = 
                await unitOfWork.GetAll<HardwareFunction>().Where(x => x.FunctionType == HardwareFunctionType.Valve && x.IsActivated).ToListAsync();
            foreach (HardwareFunction valveHardwareFunction in allActivatedValves)
            {
                valveHardwareFunction.IsActivated = false;
            }

            unitOfWork.Commit();
        }

        public async Task<IEnumerable<HardwareFunctionItemViewModel>> GetBasicHardwareFunctions()
        {
            IEnumerable<HardwareFunction> basicHardwareFunctions =
                await unitOfWork.GetAll<HardwareFunction>().Where(x => x.FunctionType == HardwareFunctionType.StrikeOn || 
                    x.FunctionType == HardwareFunctionType.StrikeOff || x.FunctionType == HardwareFunctionType.HoldOn).ToListAsync();

            return basicHardwareFunctions.Select(x => new HardwareFunctionItemViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Number = x.Number,
                ApiUrl = x.ApiUrl,
                FunctionType = x.FunctionType,
            });
        }

        public async Task<HardwareFunctionItemViewModel> GetDeactivateAllValvesHardwareFunction()
        {
            return await GetHardwareFunction(HardwareFunctionType.CloseAllValves);
        }

        public async Task<HardwareFunctionItemViewModel> GetTrayOutHardwareFunction()
        {
            return await GetHardwareFunction(HardwareFunctionType.TrayOut);
        }

        public async Task<HardwareFunctionItemViewModel> GetTrayInHardwareFunction()
        {
            return await GetHardwareFunction(HardwareFunctionType.TrayIn);
        }

        public async Task<HardwareFunctionItemViewModel> GetTrayLightOnHardwareFunction()
        {
            return await GetHardwareFunction(HardwareFunctionType.TrayLightOn);
        }

        public async Task<HardwareFunctionItemViewModel> GetHardwareFunction(HardwareFunctionType hardwareFunctionType)
        {
            HardwareFunction trayOutHardwareFunction =
                await unitOfWork.GetAll<HardwareFunction>().SingleAsync(x => x.FunctionType == hardwareFunctionType);

            return new HardwareFunctionItemViewModel()
            {
                Id = trayOutHardwareFunction.Id,
                Name = trayOutHardwareFunction.Name,
                Number = trayOutHardwareFunction.Number,
                ApiUrl = trayOutHardwareFunction.ApiUrl,
                FunctionType = trayOutHardwareFunction.FunctionType,
            };
        }
    }
}