using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Business.Contracts.Services.GeneSynthesizer;
using Business.Contracts.ViewModels.Common;

namespace AutoGene.Presentation.Host.Controllers.GeneSynthesizer
{
    public class GeneSynthesisActivityController : ApiController
    {
        private readonly IGeneSynthesizerService geneSynthesizerService;

        public GeneSynthesisActivityController(IGeneSynthesizerService geneSynthesizerService)
        {
            this.geneSynthesizerService = geneSynthesizerService;
        }

        public async Task<IEnumerable<SynthesisActivityItemViewModel>> Get()
        {
            var currentSynthesisActivities = await geneSynthesizerService.GetCurrentSynthesisActivities();
            return currentSynthesisActivities;
        }

        public async Task Put(IEnumerable<SynthesisActivityItemViewModel> items)
        {
            foreach (SynthesisActivityItemViewModel item in items)
            {
                await geneSynthesizerService.CreateOrUpdateSynthesisActivity(item);
            }
        }

        public async Task Delete(IEnumerable<SynthesisActivityItemViewModel> itemsToDelete)
        {
            foreach (SynthesisActivityItemViewModel item in itemsToDelete)
            {
                await geneSynthesizerService.DeleteSynthesisActivity(item.Id);
            }
        }
    }
}