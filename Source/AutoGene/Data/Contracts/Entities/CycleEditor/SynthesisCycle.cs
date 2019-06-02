using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Contracts.Entities.Identity;

namespace Data.Contracts.Entities.CycleEditor
{
    public class SynthesisCycle : Entity
    {
        public SynthesisCycle()
        {
            CycleSteps = new HashSet<CycleStep>();
        }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        public string UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<CycleStep> CycleSteps { get; set; }
    }
}