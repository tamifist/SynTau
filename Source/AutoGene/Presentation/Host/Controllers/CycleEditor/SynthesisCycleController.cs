using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Business.Contracts.Services.CycleEditor;
using Business.Contracts.ViewModels.Common;
using Business.Contracts.ViewModels.CycleEditor;

namespace AutoGene.Presentation.Host.Controllers.CycleEditor
{
    public class SynthesisCycleController : ApiController
    {
        private readonly ICycleEditorService cycleEditorService;

        public SynthesisCycleController(ICycleEditorService cycleEditorService)
        {
            this.cycleEditorService = cycleEditorService;
        }

        public async Task<IEnumerable<SynthesisCycleItemViewModel>> Get()
        {
            var synthesisCycleItems = await cycleEditorService.GetSynthesisCycles();
            return synthesisCycleItems;
        }

        public async Task Put(IEnumerable<SynthesisCycleItemViewModel> items)
        {
            foreach (SynthesisCycleItemViewModel synthesisCycleItemViewModel in items)
            {
                await cycleEditorService.CreateOrUpdateSynthesisCycle(synthesisCycleItemViewModel);
            }
        }

        public async Task Delete(IEnumerable<SynthesisCycleItemViewModel> itemsToDelete)
        {
            foreach (SynthesisCycleItemViewModel synthesisCycleItemViewModel in itemsToDelete)
            {
                await cycleEditorService.DeleteSynthesisCycle(synthesisCycleItemViewModel.Id);
            }
        }
    }
}