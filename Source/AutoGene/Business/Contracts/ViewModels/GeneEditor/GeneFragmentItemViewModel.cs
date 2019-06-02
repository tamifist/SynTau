namespace Business.Contracts.ViewModels.GeneEditor
{
    public class GeneFragmentItemViewModel
    {
        public string Id { get; set; }

        public string GeneId { get; set; }

        public int FragmentNumber { get; set; }

        public string OligoSequence { get; set; }

        public int OligoLength { get; set; }

        public int OverlappingLength { get; set; }

        public float Tm { get; set; }
    }
}