namespace Presentation.Common.Models
{
    public class AuthenticationResult
    {
        public static AuthenticationResult Success(bool mustChangePassword = false)
        {
            return new AuthenticationResult
            {
                IsSuccess = true,
                MustChangePassword = mustChangePassword
            };
        }

        public static AuthenticationResult Error(string message)
        {
            return new AuthenticationResult
            {
                ErrorMessage = message
            };
        }

        private AuthenticationResult()
        {
        }

        public bool IsSuccess
        {
            get;
            set;
        }

        public bool MustChangePassword
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }
    }
}