using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.Framework.Collections;

namespace Business.Contracts.ViewModels.Account
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

        public string RepeatPassword
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
    }
}