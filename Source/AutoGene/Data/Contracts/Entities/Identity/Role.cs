using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Contracts.Entities.Identity
{
    public class Role: Entity
    {
        private const int MinRoleNameLength = 4;
        private const int MaxRoleNameLength = 32;

        public Role()
        {
            Users = new HashSet<User>();
        }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(MaxRoleNameLength, MinimumLength = MinRoleNameLength)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}