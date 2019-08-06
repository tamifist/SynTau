using System;

namespace Business.Identity.Contracts.Exceptions
{
    public class PasswordStrengthException : Exception
    {
        public PasswordStrengthException()
            : base("CreateAccount_PasswordStrengthErrorMessage")
        {
        }
    }
}