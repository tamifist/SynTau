using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enum;

namespace Data.Contracts.Entities.CycleEditor
{
    public class HardwareFunction: Entity
    {
        [Required]
        [Index(IsUnique = true)]
        public int Number { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(50)]
        public string ApiUrl { get; set; }

        [Required]
        public HardwareFunctionType FunctionType { get; set; }

        [Required]
        public HttpMethodType HttpMethodType { get; set; }

        /// <summary>
        /// Used for manual testing(Manual Control)
        /// </summary>
        public bool IsActivated { get; set; }
    }
}