using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Shared.Framework.Security;
using Shared.Framework.Utilities;

namespace Presentation.Common.Security
{
    public class IdentityStorage : IIdentityStorage
    {
        private readonly TimeSpan cookieExpiration;

        public IdentityStorage(TimeSpan cookieExpiration)
        {
            this.cookieExpiration = cookieExpiration;
        }

        public async Task SaveIdentity(HttpContext httpContext, UserInfo userInfo)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userInfo.UserId),
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim("FirstName", userInfo.FirstName),
                new Claim("LastName", userInfo.LastName),
            };

            foreach (RoleInfo roleInfo in userInfo.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleInfo.Name));
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var cookieExpirationTime = userInfo.StayLoggedInToday
                ? DateTime.Now.EndOfDay()
                : DateTime.Now.Add(cookieExpiration);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = cookieExpirationTime,
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async Task ClearIdentity(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public UserInfo GetPrincipal(HttpContext httpContext)
        {
            ClaimsPrincipal userClaims = httpContext.User;
            UserInfo userInfo = new UserInfo();
            userInfo.UserId = httpContext.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;
            userInfo.Email = httpContext.User.FindFirst(c => c.Type == ClaimTypes.Email).Value;
            userInfo.FirstName = httpContext.User.FindFirst(c => c.Type == "FirstName").Value;
            userInfo.LastName = httpContext.User.FindFirst(c => c.Type == "LastName").Value;

            return userInfo;
        }
    }
}