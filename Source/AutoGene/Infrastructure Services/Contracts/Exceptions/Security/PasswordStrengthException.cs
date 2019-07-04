using System;

namespace Infrastructure.Contracts.Exceptions
{
    public class PasswordStrengthException : Exception
    {
        public PasswordStrengthException()
            : base("CreateAccount_PasswordStrengthErrorMessage")
        {
        }
    }
}