using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Contracts.ViewModels.GeneEditor;
using Data.Contracts.Entities.GeneEditor;

namespace Business.Contracts.Services.GeneEditor
{
    public interface IGeneFragmentationService
    {
        IEnumerable<GeneFragmentItemViewModel> GetGeneFragmentItems(string geneId);

        Task UpdateGeneFragments(string geneId, string dnaSequence, int oligoLength, int overlappingLength, float kPlusConcentration, float dmso);

        void UpdateGeneFragment(GeneFragmentItemViewModel geneFragmentItemViewModel);
    }
}