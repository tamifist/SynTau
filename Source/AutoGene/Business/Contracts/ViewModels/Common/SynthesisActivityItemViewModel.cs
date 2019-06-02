namespace Business.Contracts.ViewModels.Common
{
    public class SynthesisActivityItemViewModel: ItemViewModel
    {
        public int ChannelNumber { get; set; }

        public string DNASequence { get; set; }

        public int TotalTime { get; set; }

        public SynthesisCycleItemViewModel SynthesisCycle { get; set; }
    }
}