using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Contracts.Entities.CycleEditor;
using Shared.Enum;

namespace Data.Contracts.Entities.OligoSynthesizer
{
    public class OligoSynthesisActivity: Entity
    {
        [Index("IX_ChannelNumberAndOligoSynthesisProcessId", 1, IsUnique = true)]
        public int ChannelNumber { get; set; }
        
        [Required]
        public string DNASequence { get; set; }

        public SynthesisActivityStatus Status { get; set; }

        public int TotalTime { get; set; }
        
        public string ChannelApiFunctionId { get; set; }

        [Required]
        [ForeignKey("ChannelApiFunctionId")]
        public virtual HardwareFunction ChannelApiFunction { get; set; }

        public string SynthesisCycleId { get; set; }

        [Required]
        [ForeignKey("SynthesisCycleId")]
        public virtual SynthesisCycle SynthesisCycle { get; set; }

        [Index("IX_ChannelNumberAndOligoSynthesisProcessId", 2, IsUnique = true)]
        public string OligoSynthesisProcessId { get; set; }

        [Required]
        [ForeignKey("OligoSynthesisProcessId")]
        public virtual OligoSynthesisProcess OligoSynthesisProcess { get; set; }
    }
}