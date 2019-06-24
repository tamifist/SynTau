using Shared.Enum.Attributes;
using Shared.Framework.Utilities;

namespace Business.Contracts.ViewModels.GeneEditor
{
    public enum InitialSequenceType
    {
        [EnumDescription("Protein Initial Sequence")]
        ProteinInitialSequence = 0,

        [EnumDescription("DNA Initial Sequence")]
        DNAInitialSequence,
    }
}