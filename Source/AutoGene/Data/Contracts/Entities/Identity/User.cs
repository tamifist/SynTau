using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Data.Contracts.Entities.CycleEditor;
using Data.Contracts.Entities.GeneEditor;

namespace Data.Contracts.Entities.Identity
{
    public class User: Entity
    {
        public User()
        {
            Roles = new HashSet<Role>();
        }

        /// <summary>
        /// User login.
        /// </summary>
        [Required]
        [Index(IsUnique = true)]
        [StringLength(255)]
        public string Email
        {
            get;
            set;
        }

        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string Password
        {
            get;
            set;
        }

        [Required]
        public string PasswordSalt
        {
            get;
            set;
        }

        [Required]
        [StringLength(50)]
        public string FirstName
        {
            get;
            set;
        }

        [Required]
        [StringLength(50)]
        public string LastName
        {
            get;
            set;
        }

        public virtual ICollection<Role> Roles { get; set; }

        public void AddRole(Role role)
        {
            bool roleIsNotAssigned =
                Roles.FirstOrDefault(assignedRole => assignedRole.Name.Equals(role.Name, StringComparison.OrdinalIgnoreCase)) == null;

            if (roleIsNotAssigned)
            {
                Roles.Add(role);
            }
        }

        public void RemoveRoleByName(string name)
        {
            Role assignedRole = Roles.FirstOrDefault(role => role.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (assignedRole != null)
            {
                Roles.Remove(assignedRole);
            }
        }
    }
}