using System;

namespace Infrastructure.Contracts.Exceptions.Security
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
            : base("Login_UserNotFoundErrorMessage")
        {
        }
    }
}