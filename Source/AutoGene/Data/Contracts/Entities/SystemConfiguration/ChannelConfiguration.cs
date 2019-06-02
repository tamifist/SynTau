using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Contracts.Entities.CycleEditor;
using Data.Contracts.Entities.Identity;

namespace Data.Contracts.Entities.SystemConfiguration
{
    public class ChannelConfiguration : Entity
    {
        [Index("IX_ChannelNumberAndUserId", 1, IsUnique = true)]
        public int ChannelNumber { get; set; }

        [Required]
        [StringLength(1)]
        public string StartNucleotide { get; set; }

        [Index("IX_ChannelNumberAndUserId", 2, IsUnique = true)]
        public string UserId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public string HardwareFunctionId { get; set; }

        [Required]
        [ForeignKey("HardwareFunctionId")]
        public virtual HardwareFunction HardwareFunction { get; set; }
    }
}