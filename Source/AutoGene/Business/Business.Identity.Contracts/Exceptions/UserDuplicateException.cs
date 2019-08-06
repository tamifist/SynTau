using System;

namespace Business.Identity.Contracts.Exceptions
{
    public class UserDuplicateException : Exception
    {
        public UserDuplicateException()
            : base("CreateAccount_UserDuplicateErrorMessage")
        {
        }
    }
}