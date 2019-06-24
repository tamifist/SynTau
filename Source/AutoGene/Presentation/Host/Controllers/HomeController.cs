using System.Web.Mvc;
using Presentation.Common.Controllers;
using Presentation.Common.Security;

namespace AutoGene.Presentation.Host.Controllers
{
    [AuthorizeUser(UserRoles = "Guest,Admin")]
    public class HomeController : BaseController
    {
//        private readonly IHubContext sensorHubContext;
//        private readonly ISensorsService sensorsService;

        public HomeController()
        {
//            this.sensorsService = sensorsService;
//
//            sensorHubContext = GlobalHost.ConnectionManager.GetHubContext<SensorHub>();
        }

        [HttpGet]
        public ActionResult Index()
        {
            //return RedirectToAction("Index", "SystemMonitor");
            return RedirectToAction("Index", "Ecommerce");
        }
        
        public ActionResult FlotCharts()
        {
            return View("FlotCharts");
        }

        public ActionResult MorrisCharts()
        {
            return View("MorrisCharts");
        }

        public ActionResult Tables()
        {
            return View("Tables");
        }

        public ActionResult Forms()
        {
            return View("Forms");
        }

        public ActionResult Panels()
        {
            return View("Panels");
        }

        public ActionResult Buttons()
        {
            return View("Buttons");
        }

        public ActionResult Notifications()
        {
            return View("Notifications");
        }

        public ActionResult Typography()
        {
            return View("Typography");
        }

        public ActionResult Icons()
        {
            return View("Icons");
        }

        public ActionResult Grid()
        {
            return View("Grid");
        }

        public ActionResult Blank()
        {
            return View("Blank");
        }

//        public async Task UpdateWasteChannelAbsorbanceChart()
//        {
//            while (true)
//            {
//                await Task.Delay(1000);
//                SensorsResponse allSensorsResponse = await sensorsService.GetAllSensors();
//                var wasteChannelSensorResponse = allSensorsResponse.Sensors.FirstOrDefault(x => x.Type == SensorType.WasteAbsorbanceDetector1);
//                if (wasteChannelSensorResponse != null)
//                {
//                    PointViewModel wasteChannelAbsorbancePointViewModel = new PointViewModel();
//                    //wasteChannelAbsorbancePointViewModel.Time = allSensorsResponse.Time;
//                    wasteChannelAbsorbancePointViewModel.Value = wasteChannelSensorResponse.Value;
//                    sensorHubContext.Clients.All.create(wasteChannelAbsorbancePointViewModel);
//                }
//            }
//        }
    }
}