//using Android.App;
//using Android.Content;
//using Android.OS;
//using AutoGene.Mobile.Abstractions;
//using AutoGene.Mobile.Services;

//[assembly: Xamarin.Forms.Dependency(typeof(AutoGene.Mobile.Droid.Services.OligoSynthesizerBackgroundService))]
//namespace AutoGene.Mobile.Droid.Services
//{
//    [Service(Exported = true)]
//    public class OligoSynthesizerBackgroundService: Service, IOligoSynthesizerBackgroundService
//    {
//        public void Start()
//        {
//            var intent = new Intent(MainActivity.Instance, typeof(OligoSynthesizerBackgroundService));
//            MainActivity.Instance.StartService(intent);
//        }

//        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
//        {
//            StartOligoSynthesisProcess();
//            return StartCommandResult.NotSticky;
//        }

//        public override IBinder OnBind(Intent intent)
//        {
//            return null;
//        }

//        private async void StartOligoSynthesisProcess()
//        {
//            OligoSynthesizerService oligoSynthesizerService = new OligoSynthesizerService();
//            await oligoSynthesizerService.StartOligoSynthesisProcess();
//            StopSelf();
//        }
//    }
//}