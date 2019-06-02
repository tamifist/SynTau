using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Business.Contracts.Services.ChannelConfiguration;
using Business.Contracts.Services.OligoSynthesizer;
using Business.Contracts.ViewModels.Common;
using Business.Contracts.ViewModels.OligoSynthesizer;
using Business.Contracts.ViewModels.SystemConfiguration;
using Data.Contracts;
using Data.Contracts.Entities.CycleEditor;
using Data.Contracts.Entities.Identity;
using Data.Contracts.Entities.OligoSynthesizer;
using Services.Services.Common;
using Shared.Enum;
using Shared.Framework.Dependency;
using Shared.Framework.Security;
using Shared.Resources;

namespace Services.Services.OligoSynthesizer
{
    public class OligoSynthesizerService: BaseService, IOligoSynthesizerService, IDependency
    {
        private readonly IChannelConfigurationService channelConfigurationService;

        public OligoSynthesizerService(IUnitOfWork unitOfWork, IIdentityStorage identityStorage, 
            IChannelConfigurationService channelConfigurationService)
            : base(unitOfWork, identityStorage)
        {
            this.channelConfigurationService = channelConfigurationService;
        }

        public async Task StartSynthesis()
        {
            await ChangeSynthesisProcessStatus(SynthesisProcessStatus.InProgress);
        }

        public async Task StopSynthesis()
        {
            await ChangeSynthesisProcessStatus(SynthesisProcessStatus.Suspended);
        }
        
        public async Task<SynthesisProcessViewModel> GetCurrentSynthesisProcess()
        {
            OligoSynthesisProcess currentOligoSynthesisProcess = await GetCurrentOligoSynthesisProcess();

            if (currentOligoSynthesisProcess == null)
            {
                currentOligoSynthesisProcess = new OligoSynthesisProcess
                {
                    Status = SynthesisProcessStatus.NotStarted,
                    User = GetCurrentUser()
                };

                unitOfWork.InsertOrUpdate(currentOligoSynthesisProcess);
                unitOfWork.Commit();
            }
            
            return new SynthesisProcessViewModel()
            {
                Id = currentOligoSynthesisProcess.Id,
                Status = currentOligoSynthesisProcess.Status
            };
        }

        public async Task<IEnumerable<SynthesisActivityItemViewModel>> GetCurrentSynthesisActivities()
        {
            OligoSynthesisProcess currentOligoSynthesisProcess = await GetCurrentOligoSynthesisProcess();

            if (currentOligoSynthesisProcess?.OligoSynthesisActivities != null && currentOligoSynthesisProcess.OligoSynthesisActivities.Any())
            {
                IList<SynthesisActivityItemViewModel> currentSynthesisActivities = new List<SynthesisActivityItemViewModel>();

                foreach (OligoSynthesisActivity oligoSynthesisActivity in currentOligoSynthesisProcess.OligoSynthesisActivities)
                {
                    SynthesisActivityItemViewModel synthesisActivityItemViewModel = new SynthesisActivityItemViewModel();
                    synthesisActivityItemViewModel.Id = oligoSynthesisActivity.Id;
                    synthesisActivityItemViewModel.ChannelNumber = oligoSynthesisActivity.ChannelNumber;
                    synthesisActivityItemViewModel.DNASequence = oligoSynthesisActivity.DNASequence;
                    synthesisActivityItemViewModel.TotalTime = oligoSynthesisActivity.TotalTime;
                    synthesisActivityItemViewModel.SynthesisCycle = new SynthesisCycleItemViewModel()
                    {
                        Id = oligoSynthesisActivity.SynthesisCycle.Id,
                        Name = oligoSynthesisActivity.SynthesisCycle.Name
                    };

                    currentSynthesisActivities.Add(synthesisActivityItemViewModel);
                }

                return currentSynthesisActivities;
            }

            return Enumerable.Empty<SynthesisActivityItemViewModel>();
        }

        public async Task CreateOrUpdateSynthesisActivity(SynthesisActivityItemViewModel item)
        {
            OligoSynthesisActivity entity;

            OligoSynthesisProcess currentOligoSynthesisProcess = await GetCurrentOligoSynthesisProcess();

            if (string.IsNullOrWhiteSpace(item.Id))
            {
                entity = new OligoSynthesisActivity();
                entity.OligoSynthesisProcess = currentOligoSynthesisProcess;
            }
            else
            {
                entity = currentOligoSynthesisProcess.OligoSynthesisActivities.Single(x => x.Id == item.Id);
            }

            entity.DNASequence = !string.IsNullOrWhiteSpace(item.DNASequence) ? item.DNASequence.ToUpper() : "A";
            if (item.SynthesisCycle != null && !string.IsNullOrWhiteSpace(item.SynthesisCycle.Id))
            {
                entity.SynthesisCycle = unitOfWork.GetById<SynthesisCycle>(item.SynthesisCycle.Id);
            }
            else
            {
                SynthesisCycle defaultSynthesisCycle = await unitOfWork.GetAll<SynthesisCycle>()
                    .Include(x => x.CycleSteps)
                    .SingleAsync(x => x.UserId == null);
                entity.SynthesisCycle = defaultSynthesisCycle;
            }

            await AssignToChannel(entity, currentOligoSynthesisProcess.OligoSynthesisActivities);
            CalcOligoSynthesisActivityTotalTime(entity);
            
            unitOfWork.InsertOrUpdate(entity);
            unitOfWork.Commit();

            currentOligoSynthesisProcess = await GetCurrentOligoSynthesisProcess();

            currentOligoSynthesisProcess.TotalTime = currentOligoSynthesisProcess.OligoSynthesisActivities.Sum(x => x.TotalTime);

            unitOfWork.Commit();
        }

        public Task DeleteSynthesisActivity(string id)
        {
            return unitOfWork.DeleteWhere<OligoSynthesisActivity>(x => x.Id == id);
        }

        public async Task DeleteSynthesisProcess(string id)
        {
            OligoSynthesisProcess currentOligoSynthesisProcess = await GetCurrentOligoSynthesisProcess();
            currentOligoSynthesisProcess.Deleted = true;
            unitOfWork.Commit();
        }

        private void CalcOligoSynthesisActivityTotalTime(OligoSynthesisActivity oligoSynthesisActivity)
        {
            string dnaSequence =
                oligoSynthesisActivity.DNASequence.Substring(1, oligoSynthesisActivity.DNASequence.Length - 1);

            int synthesisActivityTime = 0;
            foreach (char nucleotide in dnaSequence.ToCharArray())
            {
                foreach (CycleStep cycleStep in oligoSynthesisActivity.SynthesisCycle.CycleSteps.OrderBy(x => x.Number))
                {
                    bool isCycleStepApplicable = IsCycleStepApplicable(nucleotide, cycleStep);
                    if (isCycleStepApplicable)
                    {
                        synthesisActivityTime += cycleStep.StepTime;
                    }
                }
            }

            oligoSynthesisActivity.TotalTime = synthesisActivityTime;
        }

        private bool IsCycleStepApplicable(char nucleotide, CycleStep cycleStep)
        {
            bool isCycleStepApplicable = (nucleotide == 'A' && cycleStep.A) ||
                                         (nucleotide == 'C' && cycleStep.C) ||
                                         (nucleotide == 'G' && cycleStep.G) ||
                                         (nucleotide == 'T' && cycleStep.T);

            return isCycleStepApplicable;
        }

        private async Task ChangeSynthesisProcessStatus(SynthesisProcessStatus newStatus)
        {
            OligoSynthesisProcess currentOligoSynthesisProcess = await GetCurrentOligoSynthesisProcess();
            currentOligoSynthesisProcess.Status = newStatus;

            unitOfWork.InsertOrUpdate(currentOligoSynthesisProcess);
            unitOfWork.Commit();
        }

        private async Task AssignToChannel(OligoSynthesisActivity currentSynthesisActivity, IEnumerable<OligoSynthesisActivity> otherSynthesisActivities)
        {
            IEnumerable<ChannelConfigurationItemViewModel> allChannels = await channelConfigurationService.GetChannelConfigurations();

            if (allChannels.Any(x => x.ChannelNumber == currentSynthesisActivity.ChannelNumber &&
                                     currentSynthesisActivity.DNASequence.StartsWith(x.StartNucleotide)))
            {
                return;
            }

            currentSynthesisActivity.ChannelNumber = 0;

            IEnumerable<ChannelConfigurationItemViewModel> availableChannels = 
                allChannels.Where(x => currentSynthesisActivity.DNASequence.StartsWith(x.StartNucleotide));
            foreach (ChannelConfigurationItemViewModel availableChannelConfiguration in availableChannels)
            {
                if (otherSynthesisActivities.All(x => x.ChannelNumber != availableChannelConfiguration.ChannelNumber))
                {
                    currentSynthesisActivity.ChannelNumber = availableChannelConfiguration.ChannelNumber;
                    currentSynthesisActivity.ChannelApiFunction = unitOfWork.GetById<HardwareFunction>(availableChannelConfiguration.HardwareFunction.Id);
                }
            }

            if (currentSynthesisActivity.ChannelNumber < 1)
            {
                throw new Exception(string.Format(AppResources.ChannelNotConfigured, currentSynthesisActivity.DNASequence[0]));
            }
        }

        private async Task<OligoSynthesisProcess> GetCurrentOligoSynthesisProcess()
        {
            var currentPrincipal = identityStorage.GetPrincipal();
            OligoSynthesisProcess currentOligoSynthesisProcess = await unitOfWork.GetAll<OligoSynthesisProcess>()
                .Include(x => x.User)
                .Include(x => x.OligoSynthesisActivities.Select(y => y.SynthesisCycle.CycleSteps))
                .Where(x => !x.Deleted && x.UserId == currentPrincipal.UserId && (x.Status == SynthesisProcessStatus.NotStarted || 
                    x.Status == SynthesisProcessStatus.InProgress || x.Status == SynthesisProcessStatus.Suspended))
                .SingleOrDefaultAsync();

            return currentOligoSynthesisProcess;
        }
    }
}