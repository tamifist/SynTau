using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contracts.ViewModels.SystemMonitor
{
    public class SystemMonitorViewModel
    {
        public SystemMonitorViewModel()
        {
            WasteChannelAbsorbancePoints = new List<PointViewModel>();
            TargetChannelAbsorbancePoints = new List<PointViewModel>();
            EnvironmentTemperaturePoints = new List<PointViewModel>();
        }

        public DateTimeOffset LastCreatedAt { get; set; }

        public IList<PointViewModel> WasteChannelAbsorbancePoints { get; set; }

        public IList<PointViewModel> TargetChannelAbsorbancePoints { get; set; }

        public IList<PointViewModel> EnvironmentTemperaturePoints { get; set; }
    }
}
