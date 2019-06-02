using System;

namespace Shared.Framework.Exceptions
{
    public class UserDuplicateException : Exception
    {
        private const string UserAlreadyExistsErrorMsg = "Another user already exists in the system with the same login name.";

        /// <summary>
        /// Initializes new class of UserDuplicateException
        /// </summary>
        public UserDuplicateException()
            : base(UserAlreadyExistsErrorMsg)
        {
        }
    }
}