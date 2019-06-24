using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using Shared.Framework.Security;

namespace Presentation.Common.Security
{
    /// <summary>
    ///     Represents an attribute that is used to restrict access by callers to an action method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        private static readonly char[] _splitParameter = new char[1]
        {
            ','
        };

        private string[] _rolesSplit = new string[0];
        private string _roles;

        public string UserRoles
        {
            get { return _roles ?? string.Empty; }
            set
            {
                _roles = value;
                _rolesSplit = SplitString(value);
            }
        }

        /// <summary>
        ///     Entry point for authorization checks.
        /// </summary>
        /// <param name="httpContext">
        ///     The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.
        /// </param>
        /// <returns>True if the user is authorized. Otherwise, false.</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            if (_rolesSplit.Length == 0)
            {
                return true;
            }

            //            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //            var autoGenePrincipal = serializer.Deserialize<PrincipalSerializeModel>(
            //                ((System.Web.Security.FormsIdentity)((System.Security.Principal.GenericPrincipal)HttpContext.Current.User).Identity).Ticket.UserData);
            IAutoGenePrincipal autoGenePrincipal = IdentityStorageHelper.GetPrincipal(HttpContext.Current);
            var role = autoGenePrincipal.Roles.SingleOrDefault(x => _rolesSplit.Contains(x.Name));
            bool isInRole = role != null;

            return isInRole;
        }

        /// <summary>
        /// Processes HTTP requests that fail authorization.
        /// </summary>
        /// <param name="filterContext">
        /// Encapsulates the information for using AuthorizeAttribute. The filterContext object contains the controller, 
        /// HTTP context, request context, action result, and route data.
        /// </param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                filterContext.HttpContext.Response.End();
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new
                    {
                        controller = "Identity",
                        action = "Login"
                    })
                );
            }
        }

        internal static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
                return new string[0];
            return Enumerable.ToArray<string>(Enumerable.Select(Enumerable.Where(Enumerable.Select((IEnumerable<string>)original.Split(AuthorizeUserAttribute._splitParameter), piece => new
            {
                piece = piece,
                trimmed = piece.Trim()
            }), param0 => !string.IsNullOrEmpty(param0.trimmed)), param0 => param0.trimmed));
        }
    }
}