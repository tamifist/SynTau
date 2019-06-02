using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Contracts.Entities.SystemMonitor
{
    public class Sensor : Entity
    {
        public SensorType Type { get; set; }

        public SensorState State { get; set; }

        public float Value { get; set; }

        public string SensorListingId { get; set; }

        [Required]
        [ForeignKey("SensorListingId")]
        public virtual SensorListing SensorListing { get; set; }
    }
}