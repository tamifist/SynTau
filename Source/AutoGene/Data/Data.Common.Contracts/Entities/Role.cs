using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Common.Contracts.Attributes;

namespace Data.Common.Contracts.Entities
{
    public class Role: BaseEntity
    {
        private const int MinRoleNameLength = 4;
        private const int MaxRoleNameLength = 32;

        [Required]
        [Index(IsUnique = true)]
        [StringLength(MaxRoleNameLength, MinimumLength = MinRoleNameLength)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string Description { get; set; }

        public ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
    }
}