using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Contracts.Entities.Identity;
using Shared.Enum;

namespace Data.Contracts.Entities.OligoSynthesizer
{
    public class OligoSynthesisProcess: Entity
    {
        public OligoSynthesisProcess()
        {
            OligoSynthesisActivities = new HashSet<OligoSynthesisActivity>();
        }

        public SynthesisProcessStatus Status { get; set; }

        public int TotalTime { get; set; }

        public string UserId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<OligoSynthesisActivity> OligoSynthesisActivities { get; set; }
    }
}