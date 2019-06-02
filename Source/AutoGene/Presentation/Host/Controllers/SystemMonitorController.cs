using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Business.Contracts.Services.SystemMonitor;
using Business.Contracts.ViewModels.SystemMonitor;
using Presentation.Common.Controllers;
using Presentation.Common.Security;

namespace AutoGene.Presentation.Host.Controllers
{
    [AuthorizeUser(UserRoles = "Guest,Admin")]
    public class SystemMonitorController : BaseController
    {
        private readonly ISystemMonitorService systemMonitorService;

        public SystemMonitorController(ISystemMonitorService systemMonitorService)
        {
            this.systemMonitorService = systemMonitorService;
        }

        public async Task<ActionResult> Index()
        {
            SystemMonitorViewModel systemMonitorViewModel = await systemMonitorService.GetSystemMonitorViewModel(DateTimeOffset.MinValue);

            return View(systemMonitorViewModel);
        }

        [HttpGet]
        public async Task<JsonResult> GetSystemMonitorViewModel(DateTimeOffset? lastCreatedAt)
        {
            SystemMonitorViewModel systemMonitorViewModel = await systemMonitorService.GetSystemMonitorViewModel(lastCreatedAt ?? DateTimeOffset.MinValue);

            return Json(systemMonitorViewModel, JsonRequestBehavior.AllowGet);
        }
    }
}