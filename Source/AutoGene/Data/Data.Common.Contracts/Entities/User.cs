using System.Collections.Generic;
using System.Linq;
using Shared.Enum;

namespace Data.Common.Contracts.Entities
{
    public class User: BaseEntity
    {
        /// <summary>
        /// User login.
        /// </summary>
        //[Required]
        //[Index(IsUnique = true)]
        //[StringLength(255)]
        public string Email
        {
            get;
            set;
        }

        //[Required]
        //[StringLength(255, MinimumLength = 8)]
        public string Password
        {
            get;
            set;
        }

        //[Required]
        public string PasswordSalt
        {
            get;
            set;
        }

        //[Required]
        //[StringLength(50)]
        public string FirstName
        {
            get;
            set;
        }

        //[Required]
        //[StringLength(50)]
        public string LastName
        {
            get;
            set;
        }

        //[Required]
        //[StringLength(255)]
        public string Organization
        {
            get;
            set;
        }

        //[Required]
        //[StringLength(255)]
        public string LabGroup
        {
            get;
            set;
        }

        //[Required]
        public CountryEnum? Country
        {
            get;
            set;
        }

        public ICollection<UserRole> UserRoles { get; } = new List<UserRole>();

        public void AddRole(Role role)
        {
            var roleIsNotAssigned = UserRoles.FirstOrDefault(userRole => userRole.Role.Name == role.Name) == null;
            if (roleIsNotAssigned)
            {
                UserRoles.Add(new UserRole { User = this, Role = role });
            }
        }

        public void RemoveRoleByName(string name)
        {
            UserRole assignedRole = UserRoles.FirstOrDefault(userRole => userRole.Role.Name == name);
            if (assignedRole != null)
            {
                UserRoles.Remove(assignedRole);
            }
        }
    }
}