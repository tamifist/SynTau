using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.Framework.Collections;
using Shared.Framework.Utilities;

namespace Business.Contracts.ViewModels.GeneEditor
{
    public class GeneEditorViewModel
    {
        public IEnumerable<ListItem> AllInitialSequenceTypes => new List<ListItem>
        {
            new ListItem() {
                Value = (int)InitialSequenceType.ProteinInitialSequence,
                Text = InitialSequenceType.ProteinInitialSequence.GetDescription()
            },
            new ListItem() {
                Value = (int)InitialSequenceType.DNAInitialSequence,
                Text = InitialSequenceType.DNAInitialSequence.GetDescription()
            },
        };

        public InitialSequenceType SelectedInitialSequenceType { get; set; }

        public IEnumerable<ListItem> AllOrganisms { get; set; }

        public string SelectedOrganismId { get; set; }

        public string SelectedOrganismName { get; set; }

        [Required]
        public string InitialSequence { get; set; }

        [Required]
        public string Name { get; set; }

        public string OptimizedDNASequence { get; set; }

        public float KPlusConcentration { get; set; }

        public float DMSO { get; set; }

        public bool IsGeneOptimized { get; set; }

        public string GeneId { get; set; }
        
        public int GeneFragmentLength { get; set; }

        public int GeneFragmentOverlappingLength { get; set; }
    }
}