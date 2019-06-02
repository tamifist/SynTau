using Shared.DTO.Responses.Common;
using Shared.Enum;

namespace Shared.DTO.Responses.GeneSynthesizer
{
    public class GeneSynthesisActivity : BaseDTO
    {
        public int ChannelNumber { get; set; }

        public string ChannelApiFunctionId { get; set; }

        public virtual HardwareFunction ChannelApiFunction { get; set; }

        public string DNASequence { get; set; }

        public SynthesisActivityStatus Status { get; set; }

        public string SynthesisCycleId { get; set; }

        public virtual SynthesisCycle SynthesisCycle { get; set; }

        public int TotalTime { get; set; }
    }
}