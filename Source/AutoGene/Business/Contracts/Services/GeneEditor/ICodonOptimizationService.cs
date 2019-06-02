using System.Threading.Tasks;

namespace Business.Contracts.Services.GeneEditor
{
    public interface ICodonOptimizationService
    {
        Task<string> OptimizeProteinSequence(string proteinSequence, string organismId);

        Task<string> OptimizeDNASequence(string dnaSequence, string organismId);
    }
}