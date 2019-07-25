using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Common.Contracts.Attributes;

namespace Data.Common.Contracts.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        [Key]
        public string Id { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        [Index(IsClustered = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset? CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? UpdatedAt { get; set; }

        public bool Deleted { get; set; }
    }
}