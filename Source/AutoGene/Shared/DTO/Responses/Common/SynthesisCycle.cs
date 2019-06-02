using System.Collections.Generic;

namespace Shared.DTO.Responses.Common
{
    public class SynthesisCycle : BaseDTO
    {
        public string Name { get; set; }

        public IEnumerable<CycleStep> CycleSteps { get; set; }
    }
}