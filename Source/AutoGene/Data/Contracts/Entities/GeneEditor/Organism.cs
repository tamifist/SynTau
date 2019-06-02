using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Contracts.Entities.GeneEditor
{
    public class Organism: Entity
    {
        public Organism()
        {
            CodonUsages = new HashSet<CodonUsage>();
        }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(128)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }

        public virtual ICollection<CodonUsage> CodonUsages { get; set; }
    }
}