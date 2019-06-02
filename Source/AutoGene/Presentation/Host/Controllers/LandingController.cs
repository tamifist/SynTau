using System.Web.Mvc;

namespace AutoGene.Presentation.Host.Controllers
{
    public class LandingController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "SynTau";
            return View("Index");
        }
    }
}