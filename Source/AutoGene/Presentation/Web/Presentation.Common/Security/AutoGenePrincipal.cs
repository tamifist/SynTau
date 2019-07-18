using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Shared.Framework.Security;

namespace Presentation.Common.Security
{
    public class AutoGenePrincipal : IAutoGenePrincipal
    {
        public AutoGenePrincipal(string userName)
        {
            Identity = new GenericIdentity(userName);
        }

        /// <summary>
        ///     Instance of IIdentity for user.
        /// </summary>
        public IIdentity Identity { get; }

        /// <summary>
        ///     User`s unique identifier
        /// </summary>
        public string UserId
        {
            get;
            set;
        }

        /// <summary>
        ///     Username
        /// </summary>
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// Roles assigned to the current principal.
        /// </summary>
        public IList<RoleInfo> Roles
        {
            get;
            set;
        }

        /// <summary>
        /// User First Name
        /// </summary>
        public string FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// User Last Name
        /// </summary>
        public string LastName
        {
            get;
            set;
        }

        /// <summary>
        /// User`s language code
        /// </summary>
        public string LanguageCode
        {
            get;
            set;
        }

        /// <summary>
        /// Current User(company) Time Zone name.
        /// </summary>
        public string Timezone
        {
            get;
            set;
        }

        public bool IsInRole(string roleName)
        {
            var role = Roles.SingleOrDefault(x => x.Name == roleName);
            return role != null;
        }
    }
}