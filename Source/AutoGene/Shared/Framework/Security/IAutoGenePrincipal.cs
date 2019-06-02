using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Shared.Framework.Security
{
    /// <summary>
    /// Interface for Auto Gene IPrinciple with additional specific properties
    /// </summary>
    public interface IAutoGenePrincipal : IPrincipal
    {
        string UserId
        {
            get;
            set;
        }

        /// <summary>
        ///     Username
        /// </summary>
        string Email
        {
            get;
            set;
        }

        /// <summary>
        /// Roles assigned to the current principal.
        /// </summary>
        IList<RoleInfo> Roles
        {
            get;
            set;
        }

        /// <summary>
        /// User First Name
        /// </summary>
        string FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// User Last Name
        /// </summary>
        string LastName
        {
            get;
            set;
        }

        /// <summary>
        /// User`s language code
        /// </summary>
        string LanguageCode
        {
            get;
            set;
        }

        /// <summary>
        /// Current User(company) Time Zone name.
        /// </summary>
        string Timezone
        {
            get;
            set;
        }
    }
}