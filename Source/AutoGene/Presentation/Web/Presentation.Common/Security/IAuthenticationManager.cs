using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Common.Security
{
    /// <summary>
    /// Provides operation related to user authentication.
    /// </summary>
    public interface IAuthenticationManager
    {
        /// <summary>
        /// Logs in the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="stayLoggedInToday">Stay logged-in today.</param>
        /// <returns></returns>
        Task<AuthenticationResult> LogIn(HttpContext httpContext, string userName, string password, bool stayLoggedInToday = false);

        /// <summary>
        /// Logs out the user.
        /// </summary>
        Task LogOut(HttpContext httpContext);

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="currentPassword">The current password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        AuthenticationResult ChangePassword(string currentPassword, string newPassword);
    }
}