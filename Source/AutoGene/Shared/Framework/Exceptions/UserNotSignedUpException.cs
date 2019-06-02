using System;

namespace Shared.Framework.Exceptions
{
    public class UserNotSignedUpException : Exception
    {
        private const string UserNotSignedUpMsg = "User not signed up.";

        /// <summary>
        /// Initializes new class of UserNotSignedUpException
        /// </summary>
        public UserNotSignedUpException()
            : base(UserNotSignedUpMsg)
        {
        }
    }
}