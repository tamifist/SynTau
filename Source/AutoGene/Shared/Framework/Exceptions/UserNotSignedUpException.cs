using System;

namespace Shared.Framework.Exceptions
{
    public class UserNotSignedUpException : Exception
    {
        private const string UserNotSignedUpMsg = "Invalid Email or Password.";

        /// <summary>
        /// Initializes new class of UserNotSignedUpException
        /// </summary>
        public UserNotSignedUpException()
            : base(UserNotSignedUpMsg)
        {
        }
    }
}