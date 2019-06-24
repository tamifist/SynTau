using System;

namespace Shared.Framework.Exceptions
{
    public class UserDuplicateException : Exception
    {
        private const string UserAlreadyExistsErrorMsg = "Email has already been taken.";

        /// <summary>
        /// Initializes new class of UserDuplicateException
        /// </summary>
        public UserDuplicateException()
            : base(UserAlreadyExistsErrorMsg)
        {
        }
    }
}