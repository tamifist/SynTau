using System.Web.Mvc;
using Presentation.Common.Controllers;
using Presentation.Common.Security;

namespace AutoGene.Presentation.Host.Controllers
{
    public class AboutUsController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Auto Gene";
            return View("Index");
        }
    }
}