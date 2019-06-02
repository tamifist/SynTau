using System.Web.Http;
using System.Web.Http.Controllers;
using Data.Services;
using Microsoft.Azure.Mobile.Server.Config;

namespace Mobile.Backend.Controllers.ApiControllers
{
    [MobileAppController]
    public abstract class BaseApiController : ApiController
    {
        protected CommonDbContext DbContext { get; set; }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DbContext = new CommonDbContext();
        }
    }
}