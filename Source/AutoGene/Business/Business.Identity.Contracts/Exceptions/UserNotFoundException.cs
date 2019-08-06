using System;

namespace Business.Identity.Contracts.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
            : base("Login_UserNotFoundErrorMessage")
        {
        }
    }
}