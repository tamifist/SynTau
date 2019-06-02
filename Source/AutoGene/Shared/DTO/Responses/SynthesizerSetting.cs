namespace Shared.DTO.Responses
{
    public class SynthesizerSetting : BaseDTO
    {
        public string AppServiceUrl { get; set; }
        
        public string SynthesizerApiUrl { get; set; }

        public int DelayAfterStrikeOn { get; set; }
    }
}