using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Business.Contracts.Services.SystemMonitor;
using Data.Contracts;
using Data.Contracts.Entities.SystemMonitor;
using Shared.Framework.Dependency;

namespace Services.Services.SystemMonitor
{
    public class SensorsService: ISensorsService, IDependency
    {
        private readonly IUnitOfWork unitOfWork;

        public SensorsService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SensorListing>> GetAllSensors(DateTimeOffset lastCreatedAt)
        {
            var result = await unitOfWork.GetAll<SensorListing>().Include(x => x.Sensors)
                .Where(x => x.CreatedAt > lastCreatedAt).OrderByDescending(x => x.CreatedAt).Take(100).OrderBy(x => x.CreatedAt).ToListAsync();
            return result;
        }
    }
}