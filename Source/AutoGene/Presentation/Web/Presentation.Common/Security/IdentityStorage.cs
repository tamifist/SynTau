using System;
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

        public void SaveIdentity(UserInfo userInfo)
        {
            var cookieExpirationTime = userInfo.StayLoggedInToday
                ? DateTime.Now.EndOfDay()
                : DateTime.Now.Add(cookieExpiration);

            //HttpCookie cookie = CreateCookie(userInfo, DateTime.Now, cookieExpirationTime);
            //HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void ClearIdentity()
        {
            //FormsAuthentication.SignOut();
        }

        public IAutoGenePrincipal GetPrincipal()
        {
//            if (HttpContext.Current == null)
//            {
//                return null;
//            }
//
//            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
//            if (cookie == null)
//            {
//                return null;
//            }
//
//            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(cookie.Value);
//
//            JavaScriptSerializer serializer = new JavaScriptSerializer();
//
//            PrincipalSerializeModel serializeModel = serializer.Deserialize<PrincipalSerializeModel>(authTicket.UserData);
//
//            AutoGenePrincipal principal = new AutoGenePrincipal(authTicket.Name);
//            principal.UserId = serializeModel.UserId;
//            principal.Email = serializeModel.Email;
//            principal.FirstName = serializeModel.FirstName;
//            principal.LastName = serializeModel.LastName;
//            principal.Roles = serializeModel.Roles;
//
//            return principal;
            return null;
        }

//        private HttpCookie CreateCookie(UserInfo userInfo, DateTime issueDate, DateTime expiration)
//        {
//            PrincipalSerializeModel serializeModel = new PrincipalSerializeModel();
//            serializeModel.UserId = userInfo.UserId;
//            serializeModel.Email = userInfo.Email;
//            serializeModel.FirstName = userInfo.FirstName;
//            serializeModel.LastName = userInfo.LastName;
//            serializeModel.Roles = userInfo.Roles;
//
//            JavaScriptSerializer serializer = new JavaScriptSerializer();
//
//            string userData = serializer.Serialize(serializeModel);
//
//            var authTicket = new FormsAuthenticationTicket(1, userInfo.Email, issueDate, expiration, true, userData);
//
//            string encTicket = FormsAuthentication.Encrypt(authTicket);
//            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
//            if (authTicket.IsPersistent)
//            {
//                authCookie.Expires = authTicket.Expiration;
//            }
//
//            return authCookie;
//        }
    }
}