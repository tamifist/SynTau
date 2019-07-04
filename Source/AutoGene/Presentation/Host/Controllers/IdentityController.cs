﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Business.Contracts.ViewModels.Common;
using Data.Contracts;
using Data.Contracts.Entities.Identity;
using Infrastructure.Contracts.Services.Security;
using Infrastructure.Contracts.ViewModels.Security;
using Presentation.Common.Controllers;
using Presentation.Common.Models;
using Presentation.Common.Security;
using Shared.Enum;
using Shared.Enum.Attributes;
using Shared.Framework.Utilities;
using Shared.Resources;
using ListItem = Shared.Framework.Collections.ListItem;

namespace AutoGene.Presentation.Host.Controllers
{
    public class IdentityController : BaseController
    {
        private readonly IAuthenticationManager authenticationManager;
        private readonly IIdentityService identityService;
        private readonly ILocalizationManager localizationManager;

        public IdentityController(IAuthenticationManager authenticationManager, IIdentityService identityService, ILocalizationManager localizationManager)
        {
            this.authenticationManager = authenticationManager;
            this.identityService = identityService;
            this.localizationManager = localizationManager;
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
            //SetAllCountries(createAccountViewModel);

            return View(createAccountViewModel);
        }

        [HttpPost]
        public ActionResult CreateAccount(CreateAccountViewModel createAccountViewModel)
        {
            bool isAccountCreated = false;
            try
            {
                isAccountCreated = identityService.CreateAccount(createAccountViewModel);
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

            //SetAllCountries(createAccountViewModel);

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

//        private void SetAllCountries(CreateAccountViewModel createAccountViewModel)
//        {
//            createAccountViewModel.AllCountries =
//                EnumUtil.GetValues<CountryEnum>().Select(
//                    x => new ListItem { Text = localizationManager.GetLocalizedString(x.GetDescription()), Value = (int)x }).OrderBy(x => x.Text);
//        }
    }
}