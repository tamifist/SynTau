using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Contracts.Services.Common;
using Business.Contracts.ViewModels.Common;
using Business.Contracts.ViewModels.GeneSynthesizer;

namespace Business.Contracts.Services.GeneSynthesizer
{
    public interface IGeneSynthesizerService : IBaseService
    {
        Task<GeneSynthesisProcessViewModel> GetCurrentSynthesisProcess();

        Task CreateGeneSynthesisProcess(string geneId);

        Task UpdateSynthesisProcess(GeneSynthesisProcessViewModel viewModel);

        Task StartSynthesis();

        Task StopSynthesis();

        Task<IEnumerable<SynthesisActivityItemViewModel>> GetCurrentSynthesisActivities();

        Task CreateOrUpdateSynthesisActivity(SynthesisActivityItemViewModel item);

        Task DeleteSynthesisActivity(string id);

        Task DeleteSynthesisProcess(string id);
    }
}