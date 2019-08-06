using Business.Identity.Contracts.ViewModels;
using Shared.Framework.Security;

namespace Business.Identity.Contracts.Services
{
    /// <summary>
    /// Interface for Identity Service authentication.
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Validates user password.
        /// </summary>
        /// <param name="email">User name from database.</param>
        /// <param name="password">Password for validate.</param>
        /// <returns>
        /// Returns UserInfo with user validation result.
        /// </returns>
        UserInfo ValidateUserCredentials(string email, string password);

        /// <summary>
        /// Changes the user password.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns><c>true</c> if the password was changed successfully; <c>false</c> otherwize.</returns>
        bool ChangeUserPassword(string userName, string password, string newPassword);

        bool CreateAccount(CreateAccountViewModel createAccountViewModel);
    }
}