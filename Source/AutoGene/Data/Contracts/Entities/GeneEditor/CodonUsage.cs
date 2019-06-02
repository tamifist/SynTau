using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Contracts.Entities.GeneEditor
{
    public class CodonUsage: Entity
    {
        public string CodonId { get; set; }

        [Required]
        [ForeignKey("CodonId")]
        public virtual Codon Codon { get; set; }

        [Required]
        public float Frequency { get; set; }
        
        public string OrganismId { get; set; }

        [Required]
        [ForeignKey("OrganismId")]
        public virtual Organism Organism { get; set; }
    }
}