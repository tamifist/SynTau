using System.Threading.Tasks;
using Business.Contracts.ViewModels.GeneEditor;

namespace Business.Contracts.Services.GeneEditor
{
    public interface IGeneEditorService
    {
        Task<GeneEditorViewModel> GetGeneEditorViewModel();

        GeneEditorViewModel GetGeneEditorViewModel(string geneId);

        Task<GeneEditorViewModel> OptimizeDNASequence(GeneEditorViewModel geneEditorViewModel);

        Task UpdateGeneFragments(string geneId, string dnaSequence, 
            int oligoLength, int overlappingLength, float kPlusConcentration, float dmso);

        Task DeleteGene(string geneId);
    }
}