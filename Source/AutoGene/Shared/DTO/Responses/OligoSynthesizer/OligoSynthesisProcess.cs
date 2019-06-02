using System.Collections.Generic;
using Shared.Enum;

namespace Shared.DTO.Responses.OligoSynthesizer
{
    public class OligoSynthesisProcess: BaseDTO
    {
        public SynthesisProcessStatus Status { get; set; }

        public string UserId { get; set; }

        public IEnumerable<OligoSynthesisActivity> OligoSynthesisActivities { get; set; }
        
        public int TotalTime { get; set; }
    }
}