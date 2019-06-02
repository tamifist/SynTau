using Business.Contracts.ViewModels.Common;

namespace Business.Contracts.ViewModels.CycleEditor
{
    public class CycleStepItemViewModel
    {
        public int StepNumber { get; set; }

        public int FunctionNumber { get; set; }

        public HardwareFunctionItemViewModel HardwareFunction { get; set; }

        //public string FunctionName { get; set; }

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

        public string SynthesisCycleId { get; set; }

        public string CycleStepId { get; set; }

        public bool IsNew {
            get
            {
                return string.IsNullOrWhiteSpace(CycleStepId);
            }
        }
    }
}