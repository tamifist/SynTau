using Shared.Enum;

namespace Shared.DTO.Responses.Common
{
    public class HardwareFunction: BaseDTO
    {
        public int Number { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ApiUrl { get; set; }

        public HardwareFunctionType FunctionType { get; set; }

        public HttpMethodType HttpMethodType { get; set; }

        /// <summary>
        /// Used for manual testing(Manual Control)
        /// </summary>
        public bool IsActivated { get; set; }
    }
}