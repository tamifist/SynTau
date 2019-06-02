using System;
using System.Collections.Generic;

namespace Infrastructure.API.Synthesizer.Responses.SystemMonitor
{
    public class SensorListingResponse
    {
        public TimeSpan Time { get; set; }

        public IEnumerable<SensorResponse> Sensors { get; set; }
    }
}