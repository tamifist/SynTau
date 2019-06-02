using System.Linq;
using System.Web.Mvc;
using Business.Contracts.ViewModels.Account;
using Data.Contracts;
using Data.Contracts.Entities.Identity;
using Infrastructure.Contracts.Security;
using Presentation.Common.Controllers;
using Presentation.Common.Models;
using Presentation.Common.Security;
using Shared.Framework.Exceptions;

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
        [ValidateAntiForgeryToken]
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAccount(CreateAccountViewModel accountViewModel)
        {
            bool isAccountCreated = identityService.CreateAccount(
                accountViewModel.FirstName, accountViewModel.LastName, accountViewModel.Email, accountViewModel.Password);

            if (isAccountCreated)
            {
                AuthenticationResult result = LogIn(accountViewModel.Email, accountViewModel.Password, false);

                if (result.IsSuccess)
                {
                    return authenticationManager.RedirectFromLoginPage(accountViewModel.Email);
                }
            }

            return View();
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
            catch (UserNotSignedUpException userNotSignedUpException)
            {
                ModelState.AddModelError(string.Empty, userNotSignedUpException.Message);
            }

            return result;
        }
    }
}