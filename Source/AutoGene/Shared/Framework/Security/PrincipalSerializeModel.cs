using System.Collections.Generic;

namespace Shared.Framework.Security
{
    public class PrincipalSerializeModel
    {
        public string UserId
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<RoleInfo> Roles
        {
            get;
            set;
        }
    }
}