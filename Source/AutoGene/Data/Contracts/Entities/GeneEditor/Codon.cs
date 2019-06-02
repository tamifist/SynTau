using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Contracts.Entities.GeneEditor
{
    public class Codon: Entity
    {
        public Codon()
        {
            CodonUsages = new HashSet<CodonUsage>();
        }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(3)]
        public string Triplet { get; set; }

        public string AminoAcidId { get; set; }

        //[Required]
        [ForeignKey("AminoAcidId")]
        public virtual AminoAcid AminoAcid { get; set; }

        public virtual ICollection<CodonUsage> CodonUsages { get; set; }

        [Bindable(false)]
        [ScaffoldColumn(false)]
        public virtual bool IsStopCodon
        {
            get
            {
                return AminoAcidId == null;
            }
        }
    }
}