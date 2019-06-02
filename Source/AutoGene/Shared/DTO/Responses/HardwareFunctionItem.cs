using Shared.Enum;

namespace Shared.DTO.Responses
{
    public class HardwareFunctionItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public string ApiUrl { get; set; }

        public HardwareFunctionType FunctionType { get; set; }
    }
}