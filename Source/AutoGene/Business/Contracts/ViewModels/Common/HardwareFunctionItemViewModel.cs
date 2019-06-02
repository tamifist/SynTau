using Shared.Enum;

namespace Business.Contracts.ViewModels.Common
{
    public class HardwareFunctionItemViewModel: ItemViewModel
    {
        public string Name { get; set; }

        public int Number { get; set; }

        public string ApiUrl { get; set; }

        public HardwareFunctionType FunctionType { get; set; }
    }
}