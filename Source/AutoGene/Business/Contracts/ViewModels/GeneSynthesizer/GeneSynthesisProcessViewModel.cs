using System.Collections.Generic;
using Business.Contracts.ViewModels.Common;
using Shared.Enum;
using Shared.Framework.Collections;

namespace Business.Contracts.ViewModels.GeneSynthesizer
{
    public class GeneSynthesisProcessViewModel: ItemViewModel
    {
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

        public bool IsGeneSelected
        {
            get { return !string.IsNullOrEmpty(GeneId); }
        }

        public IEnumerable<ListItem> AllGenes { get; set; }

        public string SelectedGeneId { get; set; }
    }
}