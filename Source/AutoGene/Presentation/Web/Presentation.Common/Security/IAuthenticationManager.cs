using Microsoft.AspNetCore.Mvc;

namespace Presentation.Common.Security
{
    /// <summary>
    /// Provides operation related to user authentication.
    /// </summary>
    public interface IAuthenticationManager
    {
        /// <summary>
        /// Redirects from login page.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Redirect action.</returns>
        ActionResult RedirectFromLoginPage(string userName);

        /// <summary>
        /// Logs in the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="stayLoggedInToday">Stay logged-in today.</param>
        /// <returns></returns>
        AuthenticationResult LogIn(string userName, string password, bool stayLoggedInToday = false);

        /// <summary>
        /// Logs out the user.
        /// </summary>
        void LogOut();

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="currentPassword">The current password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        AuthenticationResult ChangePassword(string currentPassword, string newPassword);
    }
}