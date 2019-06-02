using System.Collections.Generic;
using System.Web.Http;
using Business.Contracts.Services.GeneEditor;
using Business.Contracts.ViewModels.GeneEditor;

namespace AutoGene.Presentation.Host.Controllers.GeneEditor
{
    public class GeneFragmentController: ApiController
    {
        private readonly IGeneFragmentationService geneFragmentationService;

        public GeneFragmentController(IGeneFragmentationService geneFragmentationService)
        {
            this.geneFragmentationService = geneFragmentationService;
        }
        
        public IEnumerable<GeneFragmentItemViewModel> Get(string geneId)
        {
            var feneFragments = geneFragmentationService.GetGeneFragmentItems(geneId);
            return feneFragments;
        }

        public void Put(IEnumerable<GeneFragmentItemViewModel> items)
        {
            foreach (GeneFragmentItemViewModel geneFragmentItemViewModel in items)
            {
                geneFragmentationService.UpdateGeneFragment(geneFragmentItemViewModel);
            }
        }

        public void Delete(IEnumerable<GeneFragmentItemViewModel> itemsToDelete)
        {
        }
    }
}