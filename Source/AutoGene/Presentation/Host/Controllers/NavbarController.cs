using System.Linq;
using System.Web.Mvc;
using AutoGene.Presentation.Host.Domain;
using Presentation.Common.Controllers;

namespace AutoGene.Presentation.Host.Controllers
{
    public class NavbarController : BaseController
    {
        // GET: Navbar
        public ActionResult Index()
        {
            var data = new Domain.Data();
            return PartialView("_Navbar", data.navbarItems().ToList());
        }
    }
}