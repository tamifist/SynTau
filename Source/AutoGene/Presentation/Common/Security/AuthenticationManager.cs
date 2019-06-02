using System.Web.Mvc;
using System.Web.Security;
using Infrastructure.Contracts.Security;
using Presentation.Common.Models;
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

        /// <summary>
        /// Redirects from login page.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// Redirect action.
        /// </returns>
        public ActionResult RedirectFromLoginPage(string userName)
        {
            return new RedirectResult(FormsAuthentication.GetRedirectUrl(userName, true));
        }

        public AuthenticationResult LogIn(string userName, string password, bool stayLoggedInToday = false)
        {
            UserInfo userInfo = identityService.ValidateUserCredentials(userName, password);
            userInfo.StayLoggedInToday = stayLoggedInToday;

            storage.SaveIdentity(userInfo);
            return AuthenticationResult.Success(userInfo.MustChangePassword);
        }

        /// <summary>
        /// Logs out the user.
        /// </summary>
        public void LogOut()
        {
            storage.ClearIdentity();
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