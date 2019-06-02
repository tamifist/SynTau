namespace Shared.DTO.Responses.GeneSynthesizer
{
    public class Gene: BaseDTO
    {
        public string Name { get; set; }

        public string DNASequence { get; set; }

        public float KPlusConcentration { get; set; }

        public float DMSO { get; set; }

        public string OrganismId { get; set; }

        public string UserId { get; set; }
    }
}