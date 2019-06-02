using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Contracts.Entities.Identity;

namespace Data.Contracts.Entities.GeneEditor
{
    public class Gene: Entity
    {
        public Gene()
        {
            GeneFragments = new HashSet<GeneFragment>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string DNASequence { get; set; }

        public float KPlusConcentration { get; set; }

        public float DMSO { get; set; }

        public string OrganismId { get; set; }

        [Required]
        [ForeignKey("OrganismId")]
        public virtual Organism Organism { get; set; }

        public string UserId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<GeneFragment> GeneFragments { get; set; }
    }
}