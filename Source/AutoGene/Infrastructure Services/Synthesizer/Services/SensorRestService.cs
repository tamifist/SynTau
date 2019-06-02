using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.API.Synthesizer.Responses.SystemMonitor;
using Shared.Resources;

namespace Infrastructure.API.Synthesizer.Services
{
    public class SensorRestService : RestService
    {
        public SensorRestService() : base(Identifiers.Environment.SynthesizerApiUrl)
		{
        }

        public async Task<SensorListingResponse> GetAllSensors()
        {
            SensorListingResponse allSensorResponse =
                await PostAsync<SensorListingResponse>("/macros/all_sensors", null);

            return allSensorResponse;
        }
    }
}