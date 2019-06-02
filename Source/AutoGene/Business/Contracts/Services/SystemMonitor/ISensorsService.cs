using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Contracts.Entities.SystemMonitor;

namespace Business.Contracts.Services.SystemMonitor
{
    public interface ISensorsService
    {
        Task<IEnumerable<SensorListing>> GetAllSensors(DateTimeOffset lastCreatedAt);
    }
}