using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Contracts.Entities.GeneEditor
{
    public class GeneFragment: Entity
    {
        [Required]
        public string OligoSequence { get; set; }

        public int OligoLength { get; set; }

        public int OverlappingLength { get; set; }

        public float Tm { get; set; }

        public int FragmentNumber { get; set; }

        public string GeneId { get; set; }

        [Required]
        [ForeignKey("GeneId")]
        public virtual Gene Gene { get; set; }
    }
}