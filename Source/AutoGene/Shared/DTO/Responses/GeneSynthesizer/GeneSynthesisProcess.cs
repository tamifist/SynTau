using System.Collections.Generic;
using Shared.Enum;

namespace Shared.DTO.Responses.GeneSynthesizer
{
    public class GeneSynthesisProcess: BaseDTO
    {
        public SynthesisProcessStatus Status { get; set; }

        public string GeneId { get; set; }

        public virtual Gene Gene { get; set; }

        public IEnumerable<GeneSynthesisActivity> GeneSynthesisActivities { get; set; }
        
        public int TotalTime { get; set; }

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
    }
}