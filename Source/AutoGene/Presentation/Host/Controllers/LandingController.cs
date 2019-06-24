using System.Web.Mvc;

namespace AutoGene.Presentation.Host.Controllers
{
    public class LandingController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}