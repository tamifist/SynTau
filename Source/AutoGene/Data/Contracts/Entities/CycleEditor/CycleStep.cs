using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Contracts.Entities.CycleEditor
{
    public class CycleStep: Entity
    {
        //[Index("IX_NumberAndSynthesisCycleId", 1, IsUnique = true)]
        [Required]
        public int Number { get; set; }

        [Required]
        public int StepTime { get; set; }

        public bool A { get; set; }
        public bool G { get; set; }
        public bool C { get; set; }
        public bool T { get; set; }
        public bool Five { get; set; }
        public bool Six { get; set; }
        public bool Seven { get; set; }

        public bool SafeStep { get; set; }

        //[Index("IX_NumberAndSynthesisCycleId", 2, IsUnique = true)]
        public string SynthesisCycleId { get; set; }

        [Required]
        [ForeignKey("SynthesisCycleId")]
        public virtual SynthesisCycle SynthesisCycle { get; set; }

        public string HardwareFunctionId { get; set; }

        [Required]
        [ForeignKey("HardwareFunctionId")]
        public virtual HardwareFunction HardwareFunction { get; set; }
    }
}