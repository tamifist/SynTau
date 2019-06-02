using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Business.Contracts.Services.OligoSynthesizer;
using Business.Contracts.ViewModels.Common;

namespace AutoGene.Presentation.Host.Controllers.OligoSynthesizer
{
    public class OligoSynthesisActivityController : ApiController
    {
        private readonly IOligoSynthesizerService oligoSynthesizerService;

        public OligoSynthesisActivityController(IOligoSynthesizerService oligoSynthesizerService)
        {
            this.oligoSynthesizerService = oligoSynthesizerService;
        }

        public async Task<IEnumerable<SynthesisActivityItemViewModel>> Get()
        {
            var currentSynthesisActivities = await oligoSynthesizerService.GetCurrentSynthesisActivities();
            return currentSynthesisActivities;
        }

        public async Task Put(IEnumerable<SynthesisActivityItemViewModel> items)
        {
            foreach (SynthesisActivityItemViewModel item in items)
            {
                await oligoSynthesizerService.CreateOrUpdateSynthesisActivity(item);
            }
        }

        public async Task Delete(IEnumerable<SynthesisActivityItemViewModel> itemsToDelete)
        {
            foreach (SynthesisActivityItemViewModel item in itemsToDelete)
            {
                await oligoSynthesizerService.DeleteSynthesisActivity(item.Id);
            }
        }
    }
}