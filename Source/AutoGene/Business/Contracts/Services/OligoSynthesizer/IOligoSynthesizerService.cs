using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Contracts.Services.Common;
using Business.Contracts.ViewModels.Common;
using Business.Contracts.ViewModels.OligoSynthesizer;

namespace Business.Contracts.Services.OligoSynthesizer
{
    public interface IOligoSynthesizerService: IBaseService
    {
        Task StartSynthesis();

        Task StopSynthesis();

        Task<SynthesisProcessViewModel> GetCurrentSynthesisProcess();

        Task<IEnumerable<SynthesisActivityItemViewModel>> GetCurrentSynthesisActivities();

        Task CreateOrUpdateSynthesisActivity(SynthesisActivityItemViewModel item);

        Task DeleteSynthesisActivity(string id);

        Task DeleteSynthesisProcess(string id);
    }
}