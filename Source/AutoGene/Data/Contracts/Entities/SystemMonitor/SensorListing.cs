using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Contracts.Entities.SystemMonitor
{
    public class SensorListing : Entity
    {
        public SensorListing()
        {
            Sensors = new HashSet<Sensor>();
        }
        
        [NotMapped]
        public TimeSpan Time { get; set; }

        [Required]
        public long TimeTicks
        {
            get
            {
                return Time.Ticks;
            }
            set
            {
                Time = TimeSpan.FromTicks(value);
            }
        }

        public ICollection<Sensor> Sensors { get; set; } 
    }
}