using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Contracts.Entities.Identity;

namespace Data.Contracts.Entities.GeneOrder
{
    public class GeneOrder: Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Sequence { get; set; }

        public string UserId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}