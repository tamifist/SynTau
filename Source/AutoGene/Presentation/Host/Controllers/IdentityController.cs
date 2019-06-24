using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Business.Contracts.ViewModels.Account;
using Business.Contracts.ViewModels.Common;
using Data.Contracts;
using Data.Contracts.Entities.Identity;
using Infrastructure.Contracts.Security;
using Presentation.Common.Controllers;
using Presentation.Common.Models;
using Presentation.Common.Security;
using Shared.Enum;
using Shared.Enum.Attributes;
using Shared.Framework.Exceptions;
using Shared.Framework.Utilities;
using ListItem = Shared.Framework.Collections.ListItem;

namespace AutoGene.Presentation.Host.Controllers
{
    public class IdentityController : BaseController
    {
        private readonly IAuthenticationManager authenticationManager;
        private readonly IIdentityService identityService;

        public IdentityController(IAuthenticationManager authenticationManager, IIdentityService identityService)
        {
            this.authenticationManager = authenticationManager;
            this.identityService = identityService;
        }

        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel user)
        {
            AuthenticationResult result = LogIn(user.Email, user.Password, user.StayLoggedInToday);
            
            if (result.IsSuccess)
            {
                return result.MustChangePassword
                           ? RedirectToAction("ChangePassword")
                           : authenticationManager.RedirectFromLoginPage(user.Email);
            }

            return View();
        }

        [HttpGet]
        public ActionResult CreateAccount()
        {
            var createAccountViewModel = new CreateAccountViewModel();
            SetAllCountries(createAccountViewModel);

            return View(createAccountViewModel);
        }

        [HttpPost]
        public ActionResult CreateAccount(CreateAccountViewModel createAccountViewModel)
        {
            if (createAccountViewModel.Password != createAccountViewModel.RepeatPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
            }
            else if (createAccountViewModel.Country == null)
            {
                ModelState.AddModelError(string.Empty, "Country is not set.");
            }
            else
            {
                bool isAccountCreated = false;
                try
                {
                    isAccountCreated = identityService.CreateAccount(
                        createAccountViewModel.FirstName, createAccountViewModel.LastName, 
                        createAccountViewModel.Email, createAccountViewModel.Password, 
                        createAccountViewModel.Organization, createAccountViewModel.LabGroup, (CountryEnum)createAccountViewModel.Country.Value);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                if (isAccountCreated)
                {
                    AuthenticationResult result = LogIn(createAccountViewModel.Email, createAccountViewModel.Password, false);

                    if (result.IsSuccess)
                    {
                        return authenticationManager.RedirectFromLoginPage(createAccountViewModel.Email);
                    }
                }
            }
            
            SetAllCountries(createAccountViewModel);

            return View(createAccountViewModel);
        }
        
        public ActionResult Logout()
        {
            authenticationManager.LogOut();
            return RedirectToAction("Index", "Landing");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        private AuthenticationResult LogIn(string email, string password, bool stayLoggedInToday)
        {
            AuthenticationResult result = AuthenticationResult.Error(string.Empty);
            try
            {
                result = authenticationManager.LogIn(email, password, stayLoggedInToday);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return result;
        }

        private void SetAllCountries(CreateAccountViewModel createAccountViewModel)
        {
            createAccountViewModel.AllCountries =
                EnumUtil.GetValues<CountryEnum>().Select(x => new ListItem { Text = x.GetDescription(), Value = (int)x }).OrderBy(x => x.Text);
        }
    }
}