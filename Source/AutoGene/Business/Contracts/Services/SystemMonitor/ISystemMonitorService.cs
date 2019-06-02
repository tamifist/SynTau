using System;
using System.Threading.Tasks;
using Business.Contracts.ViewModels.SystemMonitor;

namespace Business.Contracts.Services.SystemMonitor
{
    public interface ISystemMonitorService
    {
        Task<SystemMonitorViewModel> GetSystemMonitorViewModel(DateTimeOffset lastCreatedAt);
    }
}
