namespace Business.Contracts.ViewModels.Common
{
    public class SynthesisCycleItemViewModel : ItemViewModel
    {
        public string Name { get; set; }

        public int TotalTime { get; set; }

        public int TotalSteps { get; set; }
    }
}