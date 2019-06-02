using Business.Contracts.ViewModels.Common;
using Shared.Enum;

namespace Business.Contracts.ViewModels.OligoSynthesizer
{
    public class SynthesisProcessViewModel: ItemViewModel
    {
        public SynthesisProcessStatus Status { get; set; }
    }
}