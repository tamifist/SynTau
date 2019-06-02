namespace Shared.DTO.Responses.Common
{
    public class CycleStep: BaseDTO
    {
        public int Number { get; set; }

        public int StepTime { get; set; }

        public bool A { get; set; }
        public bool G { get; set; }
        public bool C { get; set; }
        public bool T { get; set; }
        public bool Five { get; set; }
        public bool Six { get; set; }
        public bool Seven { get; set; }

        public bool SafeStep { get; set; }

        public string HardwareFunctionId { get; set; }

        public virtual HardwareFunction HardwareFunction { get; set; }
    }
}