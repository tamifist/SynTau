using System;
using System.Collections.Generic;
using System.Linq;
using Business.Contracts.Services.SystemMonitor;
using Business.Contracts.ViewModels.SystemMonitor;
using Data.Contracts.Entities.SystemMonitor;
using Shared.Framework.Dependency;
using Shared.Framework.Utilities;
using System.Threading.Tasks;

namespace Services.Services.SystemMonitor
{
    public class SystemMonitorService: ISystemMonitorService, IDependency
    {
        private readonly ISensorsService sensorsService;

        public SystemMonitorService(ISensorsService sensorsService)
        {
            this.sensorsService = sensorsService;
        }

        public async Task<SystemMonitorViewModel> GetSystemMonitorViewModel(DateTimeOffset lastCreatedAt)
        {
            IEnumerable<SensorListing> sensorListings = await sensorsService.GetAllSensors(lastCreatedAt);
            
            SystemMonitorViewModel systemMonitorViewModel = new SystemMonitorViewModel();

            foreach (SensorListing sensorListing in sensorListings)
            {
                DateTimeOffset sensorListingTime = sensorListing.Time.ToDateTimeOffset();

                foreach (Sensor sensor in sensorListing.Sensors)
                {
                    if (sensor.Type == SensorType.WasteAbsorbanceDetector1)
                    {
                        systemMonitorViewModel.WasteChannelAbsorbancePoints.Add(new PointViewModel()
                        {
                            Time = sensorListingTime,
                            Value = sensor.Value,
                        });
                    }
                    else if (sensor.Type == SensorType.TargetAbsorbanceDetector1)
                    {
                        systemMonitorViewModel.TargetChannelAbsorbancePoints.Add(new PointViewModel()
                        {
                            Time = sensorListingTime,
                            Value = sensor.Value,
                        });
                    }
                    else if (sensor.Type == SensorType.EnvironmentTemperature)
                    {
                        systemMonitorViewModel.EnvironmentTemperaturePoints.Add(new PointViewModel()
                        {
                            Time = sensorListingTime,
                            Value = sensor.Value,
                        });
                    }
                }
            }

            SetLastCreatedAt(systemMonitorViewModel, sensorListings);

            return systemMonitorViewModel;
        }

        private void SetLastCreatedAt(SystemMonitorViewModel systemMonitorViewModel, IEnumerable<SensorListing> sensorListings)
        {
            SensorListing lastSensorListing = sensorListings.LastOrDefault();

            systemMonitorViewModel.LastCreatedAt = lastSensorListing?.CreatedAt ?? DateTimeOffset.MinValue;
        }
    }
}
