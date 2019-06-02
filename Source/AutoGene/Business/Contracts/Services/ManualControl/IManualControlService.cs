using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Contracts.ViewModels.Common;
using Business.Contracts.ViewModels.ManualControl;
using Business.Contracts.Services.Common;
using Shared.Enum;

namespace Business.Contracts.Services.ManualControl
{
    public interface IManualControlService: IBaseService
    {
        Task<ManualControlViewModel> GetManualControlViewModel();

        void ActivateValves(IEnumerable<HardwareFunctionItemViewModel> selectedValves);

        Task DeactivateAllValves();

        Task<IEnumerable<HardwareFunctionItemViewModel>> GetBasicHardwareFunctions();

        Task<HardwareFunctionItemViewModel> GetHardwareFunction(HardwareFunctionType hardwareFunctionType);
    }
}