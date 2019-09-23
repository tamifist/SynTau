using System.Threading.Tasks;
using Business.Identity.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Framework.Dependency;
using Shared.Framework.Security;

namespace Presentation.Common.Security
{
    /// <summary>
    /// Provides the actions related to authentication.
    /// </summary>
    public class AuthenticationManager : IAuthenticationManager, IScopedDependency
    {
        private readonly IIdentityService identityService;
        private readonly IIdentityStorage storage;

        public AuthenticationManager(
            IIdentityService identityService,
            IIdentityStorage storage)
        {
            this.identityService = identityService;
            this.storage = storage;
        }

        public async Task<AuthenticationResult> LogIn(HttpContext httpContext, string userName, string password, bool stayLoggedInToday = false)
        {
            UserInfo userInfo = identityService.ValidateUserCredentials(userName, password);
            userInfo.StayLoggedInToday = stayLoggedInToday;

            await storage.SaveIdentity(httpContext, userInfo);
            return AuthenticationResult.Success(userInfo.MustChangePassword);
        }

        /// <summary>
        /// Logs out the user.
        /// </summary>
        public async Task LogOut(HttpContext httpContext)
        {
            await storage.ClearIdentity(httpContext);
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="currentPassword">The current password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        public AuthenticationResult ChangePassword(string currentPassword, string newPassword)
        {
            return AuthenticationResult.Error("ChangePasswordFailed");
        }
    }
}