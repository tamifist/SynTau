using System.ComponentModel.DataAnnotations;

namespace Data.Contracts.Entities.Settings
{
    public class SynthesizerSetting : Entity
    {
        [Required]
        public string AppServiceUrl { get; set; }

        [Required]
        public string SynthesizerApiUrl { get; set; }

        public int DelayAfterStrikeOn { get; set; }
    }
}