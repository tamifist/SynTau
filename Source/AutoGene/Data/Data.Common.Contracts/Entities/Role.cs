using System.Collections.Generic;

namespace Data.Common.Contracts.Entities
{
    public class Role: BaseEntity
    {
        //[Required]
        //[Index(IsUnique = true)]
        //[StringLength(MaxRoleNameLength, MinimumLength = MinRoleNameLength)]
        public string Name { get; set; }

        //[MaxLength(128)]
        public string Description { get; set; }

        public ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
    }
}