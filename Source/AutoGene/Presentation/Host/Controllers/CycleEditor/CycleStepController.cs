using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Business.Contracts.Services.CycleEditor;
using Business.Contracts.ViewModels.CycleEditor;

namespace AutoGene.Presentation.Host.Controllers.CycleEditor
{
    public class CycleStepController : ApiController
    {
        private readonly ICycleEditorService cycleEditorService;

        public CycleStepController(ICycleEditorService cycleEditorService)
        {
            this.cycleEditorService = cycleEditorService;
        }

        public async Task<IEnumerable<CycleStepItemViewModel>> Get(string synthesisCycleId)
        {
            var cycleStepItems = await cycleEditorService.GetCycleStepItems(synthesisCycleId);
            return cycleStepItems;
        }

        public async Task Put(IEnumerable<CycleStepItemViewModel> items)
        {
            foreach (CycleStepItemViewModel cycleStepItemViewModel in items)
            {
                await cycleEditorService.CreateOrUpdateCycleStep(cycleStepItemViewModel);
            }
        }

        public async Task Delete(IEnumerable<CycleStepItemViewModel> items)
        {
            foreach (CycleStepItemViewModel item in items)
            {
                await cycleEditorService.DeleteCycleStep(item.CycleStepId);
            }
        }
    }
}