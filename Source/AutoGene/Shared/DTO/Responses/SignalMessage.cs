using Shared.Enum;

namespace Shared.DTO.Responses
{
    public class SignalMessage : BaseDTO
    {
        public string Message { get; set; }

        public string GroupId { get; set; }

        public SignalType SignalType { get; set; }
    }
}