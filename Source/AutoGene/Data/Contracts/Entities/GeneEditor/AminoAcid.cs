using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Contracts.Entities.GeneEditor
{
    public class AminoAcid: Entity
    {
        public AminoAcid()
        {
            Codons = new HashSet<Codon>();
        }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(1)]
        public string Code { get; set; }

        public virtual ICollection<Codon> Codons { get; set; }
    }
}