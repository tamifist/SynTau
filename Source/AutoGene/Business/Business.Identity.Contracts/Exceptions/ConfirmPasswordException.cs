using System;

namespace Business.Identity.Contracts.Exceptions
{
    public class ConfirmPasswordException : Exception
    {
        public ConfirmPasswordException()
            : base("CreateAccount_ConfirmPasswordErrorMessage")
        {
        }
    }
}