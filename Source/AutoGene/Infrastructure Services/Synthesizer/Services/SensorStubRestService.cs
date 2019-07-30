using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.API.Synthesizer.Responses.SystemMonitor;
using Shared.Resources;

namespace Infrastructure.API.Synthesizer.Services
{
    public class SensorStubRestService : RestService
    {
        private readonly Random random;

        public SensorStubRestService() : base(Configuration.Environment.SynthesizerApiUrl)
		{
            random = new Random();
        }

        public SensorListingResponse GetAllSensors()
        {
            //http://93.84.120.9:8888/macros/all_sensors
            //            SensorMonitor sensorMonitor = new SensorMonitor();
            //            sensorMonitor.Time = TimeSpan.FromMilliseconds(321);
            //            sensorMonitor.Sensors.Add(new Sensor() { Type = SensorType.WasteAbsorbanceDetector1, State = SensorState.Active, Value = 0.37f });
            //            sensorMonitor.Sensors.Add(new Sensor() { Type = SensorType.TargetAnsorbanceDetector1, State = SensorState.Active, Value = 0.37f });
            //            string json = JsonConvert.SerializeObject(sensorMonitor);

            //            [21:15:21]
            //        Aleksei Yantsevich: для первых двух 0-2.56
            //[21:15:29]
            //        Aleksei Yantsevich: для вторых двух 0-5
            //[21:15:34]
            //        Aleksei Yantsevich: для пятого 0-1

            SensorListingResponse allSensorResponse = new SensorListingResponse();
            TimeSpan time = DateTime.Now - new DateTime(2017, 4, 26);
            
            //allSensorResponse.Time = DateTimeOffset.UtcNow.TimeOfDay.ToString();
            allSensorResponse.Time = time;
            allSensorResponse.Sensors = new List<SensorResponse>()
            {
                new SensorResponse()
                {
                    Value = (float)GetRandomValue(0, 5),
                    Type = SensorType.WasteAbsorbanceDetector1,
                },
                new SensorResponse()
                {
                    Value = (float)GetRandomValue(0, 5),
                    Type = SensorType.TargetAbsorbanceDetector1,
                },
                new SensorResponse()
                {
                    Value = (float)GetRandomValue(21, 21.3),
                    Type = SensorType.Sensor3,
                },
            };

            return allSensorResponse;
        }

        public double GetRandomValue(double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}