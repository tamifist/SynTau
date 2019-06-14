using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Business.Contracts.Services.GeneEditor;
using Business.Contracts.ViewModels.GeneEditor;
using Presentation.Common.Controllers;
using Presentation.Common.Security;

namespace AutoGene.Presentation.Host.Controllers.CycleEditor
{
    //[AuthorizeUser(UserRoles = "Guest,Admin")]
    public class EcommerceController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}