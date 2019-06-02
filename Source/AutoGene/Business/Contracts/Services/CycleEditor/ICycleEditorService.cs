using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Contracts.ViewModels.Common;
using Business.Contracts.ViewModels.CycleEditor;

namespace Business.Contracts.Services.CycleEditor
{
    public interface ICycleEditorService
    {
        Task CreateOrUpdateSynthesisCycle(SynthesisCycleItemViewModel synthesisCycleItemViewModel);

        Task DeleteSynthesisCycle(string synthesisCycleId);

        Task<IEnumerable<SynthesisCycleItemViewModel>> GetSynthesisCycles();

        Task<IEnumerable<CycleStepItemViewModel>> GetCycleStepItems(string synthesisCycleId);

        Task CreateOrUpdateCycleStep(CycleStepItemViewModel cycleStepItemViewModel);

        Task DeleteCycleStep(string cycleStepId);
    }
}