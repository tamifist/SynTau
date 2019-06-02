namespace Infrastructure.API.Synthesizer.Responses.SystemMonitor
{
    public class SensorResponse
    {
        public SensorType Type { get; set; }

        public SensorState State { get; set; }

        public float Value { get; set; }
    }
}