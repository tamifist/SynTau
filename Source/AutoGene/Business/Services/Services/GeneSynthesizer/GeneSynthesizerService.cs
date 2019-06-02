using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Business.Contracts.Services.ChannelConfiguration;
using Business.Contracts.Services.GeneSynthesizer;
using Business.Contracts.ViewModels.Common;
using Business.Contracts.ViewModels.GeneSynthesizer;
using Business.Contracts.ViewModels.SystemConfiguration;
using Data.Contracts;
using Data.Contracts.Entities.CycleEditor;
using Data.Contracts.Entities.GeneEditor;
using Data.Contracts.Entities.GeneSynthesizer;
using Data.Contracts.Entities.Identity;
using Services.Services.Common;
using Shared.Enum;
using Shared.Framework.Collections;
using Shared.Framework.Dependency;
using Shared.Framework.Security;
using Shared.Resources;

namespace Services.Services.GeneSynthesizer
{
    public class GeneSynthesizerService: BaseService, IGeneSynthesizerService, IDependency
    {
        private readonly IChannelConfigurationService channelConfigurationService;

        public GeneSynthesizerService(IUnitOfWork unitOfWork, IIdentityStorage identityStorage, 
            IChannelConfigurationService channelConfigurationService)
            : base(unitOfWork, identityStorage)
        {
            this.channelConfigurationService = channelConfigurationService;
        }

        public async Task<GeneSynthesisProcessViewModel> GetCurrentSynthesisProcess()
        {
            GeneSynthesisProcessViewModel geneSynthesisProcessViewModel;

            GeneSynthesisProcess currentGeneSynthesisProcess = await GetCurrentGeneSynthesisProcess();

            if (currentGeneSynthesisProcess != null)
            {
                geneSynthesisProcessViewModel = new GeneSynthesisProcessViewModel()
                {
                    Id = currentGeneSynthesisProcess.Id,
                    GeneId = currentGeneSynthesisProcess.GeneId,
                    Status = currentGeneSynthesisProcess.Status,
                    TotalTime = currentGeneSynthesisProcess.TotalTime,
                    SelectedGeneId = currentGeneSynthesisProcess.GeneId,

                    DenaturationTempGeneAssembly = currentGeneSynthesisProcess.DenaturationTempGeneAssembly,
                    DenaturationTimeGeneAssembly = currentGeneSynthesisProcess.DenaturationTimeGeneAssembly,
                    AnnealingTempGeneAssembly = currentGeneSynthesisProcess.AnnealingTempGeneAssembly,
                    AnnealingTimeGeneAssembly = currentGeneSynthesisProcess.AnnealingTimeGeneAssembly,
                    ElongationTempGeneAssembly = currentGeneSynthesisProcess.ElongationTempGeneAssembly,
                    ElongationTimeGeneAssembly = currentGeneSynthesisProcess.ElongationTimeGeneAssembly,

                    DenaturationTempGeneAmplification = currentGeneSynthesisProcess.DenaturationTempGeneAmplification,
                    DenaturationTimeGeneAmplification = currentGeneSynthesisProcess.DenaturationTimeGeneAmplification,
                    AnnealingTempGeneAmplification = currentGeneSynthesisProcess.AnnealingTempGeneAmplification,
                    AnnealingTimeGeneAmplification = currentGeneSynthesisProcess.AnnealingTimeGeneAmplification,
                    ElongationTempGeneAmplification = currentGeneSynthesisProcess.ElongationTempGeneAmplification,
                    ElongationTimeGeneAmplification = currentGeneSynthesisProcess.ElongationTimeGeneAmplification,

                    NumberOfGeneAssemblyCycles = currentGeneSynthesisProcess.NumberOfGeneAssemblyCycles,
                    NumberOfGeneAmplificationCycles = currentGeneSynthesisProcess.NumberOfGeneAmplificationCycles,
                };
            }
            else
            {
                geneSynthesisProcessViewModel = new GeneSynthesisProcessViewModel()
                {
                    Status = SynthesisProcessStatus.NotStarted,
                    SelectedGeneId = "",
                };
            }
            
            User currentUser = GetCurrentUser();
            IEnumerable<Gene> allGenes = await unitOfWork.GetAll<Gene>()
                .Where(x => x.UserId == currentUser.Id)
                .ToListAsync();
            
            geneSynthesisProcessViewModel.AllGenes = allGenes.Select(x => new ListItem() { Id = x.Id, Text = x.Name });

            return geneSynthesisProcessViewModel;
        }

        public async Task CreateGeneSynthesisProcess(string geneId)
        {
            Gene gene = await unitOfWork.GetAll<Gene>().Include(x => x.GeneFragments).SingleAsync(x => x.Id == geneId);
            await AssertGeneFragmentsCorrespondsToConfiguredChannels(gene.GeneFragments);

            GeneSynthesisProcess currentGeneSynthesisProcess = await GetCurrentGeneSynthesisProcess();
            if (currentGeneSynthesisProcess != null)
            {
                currentGeneSynthesisProcess.Deleted = true;
            }
            
            GeneSynthesisProcess newGeneSynthesisProcess = new GeneSynthesisProcess
            {
                Status = SynthesisProcessStatus.NotStarted,
                Gene = gene,
            };

            unitOfWork.InsertOrUpdate(newGeneSynthesisProcess);
            
            unitOfWork.Commit();

            // create synthesis activities from gene fragments

            SynthesisCycle defaultSynthesisCycle = await unitOfWork.GetAll<SynthesisCycle>()
                .Include(x => x.CycleSteps)
                .SingleAsync(x => x.UserId == null);

            IList<GeneSynthesisActivity> geneSynthesisActivities = new List<GeneSynthesisActivity>();
            foreach (GeneFragment geneFragment in gene.GeneFragments)
            {
                GeneSynthesisActivity newSynthesisActivity = 
                    await CreateSynthesisActivity(geneFragment.OligoSequence, newGeneSynthesisProcess, defaultSynthesisCycle);
                geneSynthesisActivities.Add(newSynthesisActivity);
            }

            await AssignChannelNumbers(geneSynthesisActivities);

            await unitOfWork.InsertAll(geneSynthesisActivities);

            newGeneSynthesisProcess.TotalTime = geneSynthesisActivities.Sum(x => x.TotalTime);

            unitOfWork.Commit();
        }

        private async Task AssertGeneFragmentsCorrespondsToConfiguredChannels(IEnumerable<GeneFragment> geneFragments)
        {
            var allChannels = await channelConfigurationService.GetChannelConfigurations();
            IList<ChannelConfigurationItemViewModel> availableChannels = allChannels.ToList();
            
            if (geneFragments.Count() > availableChannels.Count)
            {
                throw new Exception(AppResources.GeneSynthesizer_ChannelsConfigurationError);
            }
            
            foreach (GeneFragment geneFragment in geneFragments)
            {
                ChannelConfigurationItemViewModel channel = availableChannels.FirstOrDefault(x => geneFragment.OligoSequence.StartsWith(x.StartNucleotide));
                if (channel == null)
                {
                    throw new Exception(AppResources.GeneSynthesizer_ChannelsConfigurationError);
                }
                else
                {
                    availableChannels.Remove(channel);
                }
            }
        }

        private async Task<GeneSynthesisActivity> CreateSynthesisActivity(
            string dnaSequence, GeneSynthesisProcess currentGeneSynthesisProcess, SynthesisCycle defaultSynthesisCycle)
        {
            GeneSynthesisActivity geneSynthesisActivity = new GeneSynthesisActivity();

            geneSynthesisActivity.GeneSynthesisProcessId = currentGeneSynthesisProcess.Id;
            geneSynthesisActivity.GeneSynthesisProcess = currentGeneSynthesisProcess;

            geneSynthesisActivity.SynthesisCycleId = defaultSynthesisCycle.Id;
            geneSynthesisActivity.SynthesisCycle = defaultSynthesisCycle;

            geneSynthesisActivity.DNASequence = !string.IsNullOrWhiteSpace(dnaSequence) ? dnaSequence.ToUpper() : "A";
            
            CalcGeneSynthesisActivityTotalTime(geneSynthesisActivity);
            
            return geneSynthesisActivity;
        }

        private async Task AssignChannelNumbers(IEnumerable<GeneSynthesisActivity> geneSynthesisActivities)
        {
            var allChannels = await channelConfigurationService.GetChannelConfigurations();
            IList<ChannelConfigurationItemViewModel> availableChannels = allChannels.ToList();

            foreach (GeneSynthesisActivity geneSynthesisActivity in geneSynthesisActivities)
            {
                ChannelConfigurationItemViewModel channel = availableChannels.FirstOrDefault(
                    x => geneSynthesisActivity.DNASequence.StartsWith(x.StartNucleotide));

                geneSynthesisActivity.ChannelNumber = channel.ChannelNumber;

                HardwareFunction channelApiFunction = unitOfWork.GetById<HardwareFunction>(channel.HardwareFunction.Id);
                geneSynthesisActivity.ChannelApiFunctionId = channelApiFunction.Id;
                geneSynthesisActivity.ChannelApiFunction = channelApiFunction;

                availableChannels.Remove(channel);
            }
        }

        public async Task UpdateSynthesisProcess(GeneSynthesisProcessViewModel viewModel)
        {
            GeneSynthesisProcess entity = await GetCurrentGeneSynthesisProcess();

            entity.DenaturationTempGeneAssembly = viewModel.DenaturationTempGeneAssembly;
            entity.DenaturationTimeGeneAssembly = viewModel.DenaturationTimeGeneAssembly;
            entity.AnnealingTempGeneAssembly = viewModel.AnnealingTempGeneAssembly;
            entity.AnnealingTimeGeneAssembly = viewModel.AnnealingTimeGeneAssembly;
            entity.ElongationTempGeneAssembly = viewModel.ElongationTempGeneAssembly;
            entity.ElongationTimeGeneAssembly = viewModel.ElongationTimeGeneAssembly;

            entity.DenaturationTempGeneAmplification = viewModel.DenaturationTempGeneAmplification;
            entity.DenaturationTimeGeneAmplification = viewModel.DenaturationTimeGeneAmplification;
            entity.AnnealingTempGeneAmplification = viewModel.AnnealingTempGeneAmplification;
            entity.AnnealingTimeGeneAmplification = viewModel.AnnealingTimeGeneAmplification;
            entity.ElongationTempGeneAmplification = viewModel.ElongationTempGeneAmplification;
            entity.ElongationTimeGeneAmplification = viewModel.ElongationTimeGeneAmplification;

            entity.NumberOfGeneAssemblyCycles = viewModel.NumberOfGeneAssemblyCycles;
            entity.NumberOfGeneAmplificationCycles = viewModel.NumberOfGeneAmplificationCycles;

            CalcGeneSynthesisProcessTotalTime(entity);

            unitOfWork.Commit();
        }

        public async Task StartSynthesis()
        {
            await ChangeSynthesisProcessStatus(SynthesisProcessStatus.InProgress);
        }

        public async Task StopSynthesis()
        {
            await ChangeSynthesisProcessStatus(SynthesisProcessStatus.Suspended);
        }
        
        public async Task<IEnumerable<SynthesisActivityItemViewModel>> GetCurrentSynthesisActivities()
        {
            GeneSynthesisProcess currentGeneSynthesisProcess = await GetCurrentGeneSynthesisProcess();

            if (currentGeneSynthesisProcess?.GeneSynthesisActivities != null && currentGeneSynthesisProcess.GeneSynthesisActivities.Any())
            {
                IList<SynthesisActivityItemViewModel> currentSynthesisActivities = new List<SynthesisActivityItemViewModel>();

                foreach (GeneSynthesisActivity geneSynthesisActivity in currentGeneSynthesisProcess.GeneSynthesisActivities)
                {
                    SynthesisActivityItemViewModel synthesisActivityItemViewModel = new SynthesisActivityItemViewModel();
                    synthesisActivityItemViewModel.Id = geneSynthesisActivity.Id;
                    synthesisActivityItemViewModel.ChannelNumber = geneSynthesisActivity.ChannelNumber;
                    synthesisActivityItemViewModel.DNASequence = geneSynthesisActivity.DNASequence;
                    synthesisActivityItemViewModel.TotalTime = geneSynthesisActivity.TotalTime;
                    synthesisActivityItemViewModel.SynthesisCycle = new SynthesisCycleItemViewModel()
                    {
                        Id = geneSynthesisActivity.SynthesisCycle.Id,
                        Name = geneSynthesisActivity.SynthesisCycle.Name
                    };

                    currentSynthesisActivities.Add(synthesisActivityItemViewModel);
                }

                return currentSynthesisActivities;
            }

            return Enumerable.Empty<SynthesisActivityItemViewModel>();
        }

        public async Task CreateOrUpdateSynthesisActivity(SynthesisActivityItemViewModel item)
        {
            GeneSynthesisActivity entity;

            GeneSynthesisProcess currentGeneSynthesisProcess = await GetCurrentGeneSynthesisProcess();

            if (string.IsNullOrWhiteSpace(item.Id))
            {
                entity = new GeneSynthesisActivity();
                entity.GeneSynthesisProcess = currentGeneSynthesisProcess;
            }
            else
            {
                entity = await unitOfWork.GetAll<GeneSynthesisActivity>()
                    .Include(x => x.SynthesisCycle)
                    .Include(x => x.GeneSynthesisProcess)
                    .SingleAsync(x => x.Id == item.Id);
            }

            entity.DNASequence = !string.IsNullOrWhiteSpace(item.DNASequence) ? item.DNASequence.ToUpper() : "A";
            if (item.SynthesisCycle != null && !string.IsNullOrWhiteSpace(item.SynthesisCycle.Id))
            {
                entity.SynthesisCycle = unitOfWork.GetById<SynthesisCycle>(item.SynthesisCycle.Id);
            }
            else
            {
                SynthesisCycle defaultSynthesisCycle = await unitOfWork.GetAll<SynthesisCycle>()
                    .SingleAsync(x => x.UserId == null);
                entity.SynthesisCycle = defaultSynthesisCycle;
            }

            await AssignToChannel(entity, currentGeneSynthesisProcess.GeneSynthesisActivities);
            CalcGeneSynthesisActivityTotalTime(entity);

            unitOfWork.InsertOrUpdate(entity);
            unitOfWork.Commit();

            currentGeneSynthesisProcess = await GetCurrentGeneSynthesisProcess();

            CalcGeneSynthesisProcessTotalTime(currentGeneSynthesisProcess);
            
            unitOfWork.Commit();
        }

        private void CalcGeneSynthesisProcessTotalTime(GeneSynthesisProcess geneSynthesisProcess)
        {
            int totalTimeActivities = geneSynthesisProcess.GeneSynthesisActivities.Sum(x => x.TotalTime);
            int totalTimeGeneAssembly =
                (geneSynthesisProcess.DenaturationTimeGeneAssembly + geneSynthesisProcess.AnnealingTimeGeneAssembly + geneSynthesisProcess.ElongationTimeGeneAssembly) * 
                geneSynthesisProcess.NumberOfGeneAssemblyCycles;
            int totalTimeGeneAmplification =
                (geneSynthesisProcess.DenaturationTimeGeneAmplification + geneSynthesisProcess.AnnealingTimeGeneAmplification + geneSynthesisProcess.ElongationTimeGeneAmplification) *
                geneSynthesisProcess.NumberOfGeneAmplificationCycles;

            geneSynthesisProcess.TotalTime = totalTimeActivities + totalTimeGeneAssembly + totalTimeGeneAmplification;
        }

        public async Task DeleteSynthesisActivity(string id)
        {
            await unitOfWork.DeleteWhere<GeneSynthesisActivity>(x => x.Id == id);

            var currentGeneSynthesisProcess = await GetCurrentGeneSynthesisProcess();

            CalcGeneSynthesisProcessTotalTime(currentGeneSynthesisProcess);

            unitOfWork.Commit();
        }

        public async Task DeleteSynthesisProcess(string id)
        {
            GeneSynthesisProcess currentGeneSynthesisProcess = await GetCurrentGeneSynthesisProcess();
            currentGeneSynthesisProcess.Deleted = true;
            
            unitOfWork.Commit();
        }

        private void CalcGeneSynthesisActivityTotalTime(GeneSynthesisActivity geneSynthesisActivity)
        {
            string dnaSequence =
                geneSynthesisActivity.DNASequence.Substring(1, geneSynthesisActivity.DNASequence.Length - 1);

            int synthesisActivityTime = 0;
            foreach (char nucleotide in dnaSequence.ToCharArray())
            {
                foreach (CycleStep cycleStep in geneSynthesisActivity.SynthesisCycle.CycleSteps.OrderBy(x => x.Number))
                {
                    bool isCycleStepApplicable = IsCycleStepApplicable(nucleotide, cycleStep);
                    if (isCycleStepApplicable)
                    {
                        synthesisActivityTime += cycleStep.StepTime;
                    }
                }
            }

            geneSynthesisActivity.TotalTime = synthesisActivityTime;
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
            GeneSynthesisProcess currentGeneSynthesisProcess = await GetCurrentGeneSynthesisProcess();
            currentGeneSynthesisProcess.Status = newStatus;

            unitOfWork.InsertOrUpdate(currentGeneSynthesisProcess);
            unitOfWork.Commit();
        }

        private async Task AssignToChannel(GeneSynthesisActivity currentSynthesisActivity, IEnumerable<GeneSynthesisActivity> otherSynthesisActivities)
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

        private async Task<GeneSynthesisProcess> GetCurrentGeneSynthesisProcess()
        {
            var currentPrincipal = identityStorage.GetPrincipal();
            GeneSynthesisProcess geneSynthesisProcess = await unitOfWork.GetAll<GeneSynthesisProcess>()
                .Include(x => x.Gene.User)
                .Include(x => x.GeneSynthesisActivities.Select(y => y.SynthesisCycle.CycleSteps))
                .Where(x => !x.Deleted && x.Gene.UserId == currentPrincipal.UserId && (x.Status == SynthesisProcessStatus.NotStarted || 
                    x.Status == SynthesisProcessStatus.InProgress || x.Status == SynthesisProcessStatus.Suspended))
                .FirstOrDefaultAsync();

            return geneSynthesisProcess;
        }
    }
}