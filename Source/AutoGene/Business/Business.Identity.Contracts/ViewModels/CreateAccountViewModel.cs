using System.Collections.Generic;
using Shared.Framework.Collections;

namespace Business.Identity.Contracts.ViewModels
{
    public class CreateAccountViewModel
    {
        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

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

        public string ConfirmPassword
        {
            get;
            set;
        }

        public string Organization
        {
            get;
            set;
        }

        public string LabGroup
        {
            get;
            set;
        }

        public int? Country
        {
            get;
            set;
        }

        public IEnumerable<ListItem> AllCountries
        {
            get;
            set;
        }

        public string ReturnUrl
        {
            get;
            set;
        }
    }
}