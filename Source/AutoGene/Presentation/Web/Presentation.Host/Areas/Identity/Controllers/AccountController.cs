using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Identity.Contracts.Services;
using Business.Identity.Contracts.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common.Controllers;
using Presentation.Common.Security;
using Shared.Resources;

namespace Presentation.Host.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : BaseController
    {
        private readonly IAuthenticationManager authenticationManager;
        private readonly IIdentityService identityService;
        private readonly ILocalizationManager localizationManager;

        public AccountController(IAuthenticationManager authenticationManager, IIdentityService identityService, 
            ILocalizationManager localizationManager)
        {
            this.authenticationManager = authenticationManager;
            this.identityService = identityService;
            this.localizationManager = localizationManager;
        }

        [HttpGet]
        public ActionResult Login(string returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel user)
        {
            AuthenticationResult result = await LogIn(user.Email, user.Password, user.StayLoggedInToday);
            
            if (result.IsSuccess)
            {
                return RedirectFromLoginPage();
            }

            return View();
        }

        [HttpGet]
        public ActionResult CreateAccount(string returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;

            var createAccountViewModel = new CreateAccountViewModel();
            //SetAllCountries(createAccountViewModel);

            return View(createAccountViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAccount(CreateAccountViewModel createAccountViewModel)
        {
            bool isAccountCreated = false;
            try
            {
                isAccountCreated = identityService.CreateAccount(createAccountViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, localizationManager.GetLocalizedString(ex.Message));
            }
            
            if (isAccountCreated)
            {
                AuthenticationResult result = await LogIn(createAccountViewModel.Email, createAccountViewModel.Password, false);

                if (result.IsSuccess)
                {
                    return RedirectFromLoginPage();
                }
            }

            //SetAllCountries(createAccountViewModel);

            return View(createAccountViewModel);
        }
        
        public ActionResult Logout()
        {
            authenticationManager.LogOut(HttpContext);
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        private async Task<AuthenticationResult> LogIn(string email, string password, bool stayLoggedInToday)
        {
            AuthenticationResult result = AuthenticationResult.Error(string.Empty);
            try
            {
                result = await authenticationManager.LogIn(HttpContext, email, password, stayLoggedInToday);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, localizationManager.GetLocalizedString(ex.Message));
            }

            return result;
        }

        private ActionResult RedirectFromLoginPage()
        {
            string redirectUrl = TempData["ReturnUrl"]?.ToString();// ?? Url.Action("Index", "Home");
            if (redirectUrl == null)
            {
                redirectUrl = Url.Action("Index", "Home", new { Area = "" });
            }
            return Redirect(redirectUrl);
        }

//        private void SetAllCountries(CreateAccountViewModel createAccountViewModel)
//        {
//            createAccountViewModel.AllCountries =
//                EnumUtil.GetValues<CountryEnum>().Select(
//                    x => new ListItem { Text = localizationManager.GetLocalizedString(x.GetDescription()), Value = (int)x }).OrderBy(x => x.Text);
//        }
    }
}