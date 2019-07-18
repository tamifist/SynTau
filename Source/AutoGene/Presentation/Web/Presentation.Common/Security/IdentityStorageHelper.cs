using Microsoft.AspNetCore.Http;
using Shared.Framework.Security;

namespace Presentation.Common.Security
{
    public static class IdentityStorageHelper
    {
        public static IAutoGenePrincipal GetPrincipal(HttpContext httpContext)
        {
//            if (httpContext == null)
//            {
//                return null;
//            }
//
//            HttpCookie cookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
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
    }
}