using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Common.Contracts.Entities;

namespace Data.Ecommerce.Contracts.Entities
{
    public class GeneOrder: BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Sequence { get; set; }

        public string UserId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}