using System;

namespace Infrastructure.Contracts.Exceptions
{
    public class ConfirmPasswordException : Exception
    {
        public ConfirmPasswordException()
            : base("CreateAccount_ConfirmPasswordErrorMessage")
        {
        }
    }
}