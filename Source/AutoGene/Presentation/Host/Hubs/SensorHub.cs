using Microsoft.AspNet.SignalR;

namespace AutoGene.Presentation.Host.Hubs
{
    public class SensorHub : Hub
    {
//        private ISensorsService sensorsService;
//
//        public SensorHub()
//        {
//            sensorsService = new SensorsService();
//        }
//
//        public async Task<IEnumerable<PointViewModel>> Read()
//        {
//            SensorListingResponse allSensorResponse = await sensorsService.GetAllSensors();
//            var wasteChannelSensorResponse = allSensorResponse.Sensors.FirstOrDefault(x => x.Type == SensorType.WasteAbsorbanceDetector1);
//            var result = new List<PointViewModel>();
//            if (wasteChannelSensorResponse != null)
//            {
//                PointViewModel wasteChannelAbsorbancePointViewModel = new PointViewModel();
//                //wasteChannelAbsorbancePointViewModel.Time = allSensorsResponse.Time;
//                wasteChannelAbsorbancePointViewModel.Value = wasteChannelSensorResponse.Value;
//                result.Add(wasteChannelAbsorbancePointViewModel);
//            }
//            
//            return result;
//        }
//
//        public PointViewModel Create(PointViewModel point)
//        {
//            Clients.Others.create(point);
//            return point;
//        }
    }
}