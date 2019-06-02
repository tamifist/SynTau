using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enum;
using Data.Contracts.Entities.GeneEditor;

namespace Data.Contracts.Entities.GeneSynthesizer
{
    public class GeneSynthesisProcess : Entity
    {
        public GeneSynthesisProcess()
        {
            GeneSynthesisActivities = new HashSet<GeneSynthesisActivity>();
        }

        /// <summary>
        /// Denaturation temperature (gene assembly)
        /// </summary>
        public float DenaturationTempGeneAssembly { get; set; }
        public int DenaturationTimeGeneAssembly { get; set; }

        /// <summary>
        /// Annealing temperature (gene assembly)
        /// </summary>
        public float AnnealingTempGeneAssembly { get; set; }
        public int AnnealingTimeGeneAssembly { get; set; }

        /// <summary>
        /// Elongation temperature (gene assembly)
        /// </summary>
        public float ElongationTempGeneAssembly { get; set; }
        public int ElongationTimeGeneAssembly { get; set; }

        /// <summary>
        /// Denaturation temperature (gene amplification)
        /// </summary>
        public float DenaturationTempGeneAmplification { get; set; }
        public int DenaturationTimeGeneAmplification { get; set; }

        /// <summary>
        /// Annealing temperature (gene amplification)
        /// </summary>
        public float AnnealingTempGeneAmplification { get; set; }
        public int AnnealingTimeGeneAmplification { get; set; }

        /// <summary>
        /// Elongation temperature (gene amplification)
        /// </summary>
        public float ElongationTempGeneAmplification { get; set; }
        public int ElongationTimeGeneAmplification { get; set; }

        public int NumberOfGeneAssemblyCycles { get; set; }

        public int NumberOfGeneAmplificationCycles { get; set; }

        public SynthesisProcessStatus Status { get; set; }

        public int TotalTime { get; set; }

        public string GeneId { get; set; }

        [Required]
        [ForeignKey("GeneId")]
        public virtual Gene Gene { get; set; }

        public virtual ICollection<GeneSynthesisActivity> GeneSynthesisActivities { get; set; }
    }
}