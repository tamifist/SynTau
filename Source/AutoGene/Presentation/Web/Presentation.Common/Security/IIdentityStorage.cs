using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shared.Framework.Security;

namespace Presentation.Common.Security
{
    /// <summary>
    /// Identity storage contract.
    /// </summary>
    public interface IIdentityStorage
    {
        /// <summary>
        /// Sets given identity.
        /// </summary>
        /// <param name="userInfo"></param>
        Task SaveIdentity(HttpContext httpContext, UserInfo userInfo);

        /// <summary>
        /// Clears principal info.
        /// </summary>
        Task ClearIdentity(HttpContext httpContext);

        /// <summary>
        /// Retrieves current principal information
        /// </summary>
        /// <returns></returns>
        UserInfo GetPrincipal(HttpContext httpContext);
    }
}