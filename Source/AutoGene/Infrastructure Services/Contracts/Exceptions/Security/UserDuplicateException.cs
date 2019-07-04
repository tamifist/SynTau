using System;

namespace Infrastructure.Contracts.Exceptions
{
    public class UserDuplicateException : Exception
    {
        public UserDuplicateException()
            : base("CreateAccount_UserDuplicateErrorMessage")
        {
        }
    }
}