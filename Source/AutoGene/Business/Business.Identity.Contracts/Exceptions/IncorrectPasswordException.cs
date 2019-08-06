using System;

namespace Business.Identity.Contracts.Exceptions
{
    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException()
            : base("Login_IncorrectPasswordErrorMessage")
        {
        }
    }
}