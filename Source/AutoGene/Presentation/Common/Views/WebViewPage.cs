using System.Web;
using Presentation.Common.Models.Json;
using Presentation.Common.Security;
using Shared.Framework.Localization;
using Shared.Framework.Security;

namespace Presentation.Common.Views
{
    /// <summary>
    /// Base class for view pages.
    /// </summary>
    /// <typeparam name="TModel">type of the view model.</typeparam>
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        private IAutoGenePrincipal userPrincipal;
        private Localizer localizer = NullLocalizer.Instance;

        public Localizer T
        {
            get
            {
                return localizer;
            }
            set
            {
                localizer = value;
            }
        }

        public Jsonizer J
        {
            get;
            set;
        }

        public IAutoGenePrincipal UserPrincipal
        {
            get
            {
                if (userPrincipal == null)
                {
                    userPrincipal = IdentityStorageHelper.GetPrincipal(HttpContext.Current);
                }

                return userPrincipal;
            }
        }
    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}