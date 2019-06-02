using System.Collections;
using System.Collections.Generic;
using Shared.Enum;
using Shared.Framework.Collections;

namespace Business.Contracts.ViewModels.ManualControl
{
    public class ManualControlViewModel
    {
        public bool IsValvesActivated { get; set; } 

        public IEnumerable<ListItem> AllSyringePumpHardwareFunctions { get; set; }

        public HardwareFunctionType SelectedSyringePumpFunction { get; set; }

        public bool SyringePumpStepMode { get; set; }

        public int SyringePumpFlow { get; set; }

        public int SyringePumpVolume { get; set; }

        public IEnumerable<ListItem> AllGSValvesHardwareFunctions { get; set; }

        public string SelectedGSValveFunction { get; set; }
    }
}