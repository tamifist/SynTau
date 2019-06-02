using Business.Contracts.ViewModels.Common;

namespace Business.Contracts.ViewModels.SystemConfiguration
{
    public class ChannelConfigurationItemViewModel : ItemViewModel
    {
        public int ChannelNumber { get; set; }

        public string StartNucleotide { get; set; }

        public HardwareFunctionItemViewModel HardwareFunction { get; set; }
    }
}