using Shared.Framework.Utilities;

namespace Business.Contracts.ViewModels.GeneEditor
{
    public enum OrganismType
    {
        [EnumDescription("Escherichia coli (E.coli)")]
        EscherichiaColi = 0,

        [EnumDescription("Homo sapiens")]
        HomoSapiens,
    }
}