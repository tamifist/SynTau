using System.Web.Mvc;
using Presentation.Common.Controllers;
using Presentation.Common.Security;

namespace AutoGene.Presentation.Host.Controllers.SystemConfiguration
{
    [AuthorizeUser(UserRoles = "Guest,Admin")]
    public class SystemConfigurationController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}