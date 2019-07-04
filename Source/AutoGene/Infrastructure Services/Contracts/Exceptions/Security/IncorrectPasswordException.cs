using System;

namespace Infrastructure.Contracts.Exceptions
{
    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException()
            : base("Login_IncorrectPasswordErrorMessage")
        {
        }
    }
}