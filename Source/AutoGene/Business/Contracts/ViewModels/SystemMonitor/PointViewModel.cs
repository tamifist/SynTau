using System;

namespace Business.Contracts.ViewModels.SystemMonitor
{
    public class PointViewModel
    {
        public DateTimeOffset Time { get; set; }

        public double Value { get; set; } 
    }
}