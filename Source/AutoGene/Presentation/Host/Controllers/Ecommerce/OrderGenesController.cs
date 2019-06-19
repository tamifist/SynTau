using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Business.Contracts.Services.GeneEditor;
using Business.Contracts.ViewModels.GeneEditor;
using Presentation.Common.Controllers;

namespace AutoGene.Presentation.Host.Controllers.Ecommerce
{
    //[AuthorizeUser(UserRoles = "Guest,Admin")]
    public class OrderGenesController : BaseController
    {
        private readonly IGeneEditorService geneEditorService;

        public OrderGenesController(IGeneEditorService geneEditorService)
        {
            this.geneEditorService = geneEditorService;
        }

        [HttpPost]
        public async Task<ActionResult> Index(string genesProjectName)
        {
            GeneEditorViewModel geneEditorViewModel = await geneEditorService.GetGeneEditorViewModel();

            return View(geneEditorViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> OptimizeGene(GeneEditorViewModel geneEditorViewModel)
        {
            //            if (string.IsNullOrWhiteSpace(geneEditorViewModel.ProteinInitialSequence) &&
            //                (string.IsNullOrWhiteSpace(geneEditorViewModel.DNAInitialSequence) || geneEditorViewModel.DNAInitialSequence.Length % 3 != 0))
            //            {
            //                ModelState.AddModelError(string.Empty, "Wrong sequence");
            //                return View(geneEditorViewModel);
            //            }

            //            ModelState.Clear();
            try
            {
                geneEditorViewModel = await geneEditorService.OptimizeDNASequence(geneEditorViewModel);
                return Json(geneEditorViewModel);
            }
            catch (Exception ex)
            {
                return JsonError();
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateGeneFragments(GeneEditorViewModel geneEditorViewModel)
        {
            await geneEditorService.UpdateGeneFragments(
                geneEditorViewModel.GeneId, geneEditorViewModel.OptimizedDNASequence,
                geneEditorViewModel.GeneFragmentLength, geneEditorViewModel.GeneFragmentOverlappingLength,
                geneEditorViewModel.KPlusConcentration, geneEditorViewModel.DMSO);

            return JsonSuccess(geneEditorViewModel.GeneId);
        }

        [HttpGet]
        public JsonResult GetGeneEditorViewModel(string geneId)
        {
            GeneEditorViewModel geneEditorViewModel = geneEditorService.GetGeneEditorViewModel(geneId);
            return Json(geneEditorViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> DeleteGene(string geneId)
        {
            await geneEditorService.DeleteGene(geneId);

            return JsonSuccess(geneId);
        }
    }
}