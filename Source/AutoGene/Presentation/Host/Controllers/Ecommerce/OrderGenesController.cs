using System.Web.Mvc;
using Presentation.Common.Controllers;

namespace AutoGene.Presentation.Host.Controllers.Ecommerce
{
    //[AuthorizeUser(UserRoles = "Guest,Admin")]
    public class OrderGenesController : BaseController
    {
        [HttpPost]
        public ActionResult Index(string genesProjectName)
        {
            return View();
        }
    }
}