using System.Collections.Generic;

namespace Shared.Framework.Security
{
    public class UserInfo
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

        public string Token
        {
            get;
            set;
        }

        public IList<RoleInfo> Roles
        {
            get;
            set;
        }

        public bool MustChangePassword
        {
            get;
            set;
        }

        public bool StayLoggedInToday
        {
            get;
            set;
        }
    }
}