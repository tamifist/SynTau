using Data.Common.Contracts.Entities;
using Shared.Enum;

namespace Data.Ecommerce.Contracts.Entities
{
    public class GeneOrder: BaseEntity
    {
        public string Name { get; set; }

        public string Sequence { get; set; }

        public SequenceType SequenceType { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}