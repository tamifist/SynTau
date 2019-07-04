namespace Infrastructure.Contracts.ViewModels.Security
{
    public class LoginViewModel
    {
        public string Email
        {
            get;
            set;
        }
        
        public string Password
        {
            get;
            set;
        }

        public bool StayLoggedInToday
        {
            get;
            set;
        }
    }
}