using AutoGene.Mobile.Abstractions;
using AutoGene.Mobile.Enums;
using AutoGene.Mobile.Helpers;
using AutoGene.Mobile.Pages;
using AutoGene.Mobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace AutoGene.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SetupServiceLocator();

            var userId = Settings.GetSetting<string>(AppSettings.UserId);
            if (string.IsNullOrEmpty(userId))
            {
                MainPage = new LoginPage();
            }
            else
            {
                MainPage = new RootPage();
            }

            //MainPage = new SystemMonitorPage();
        }

        protected override void OnStart()
        {
            MobileCenter.Start("android=90fd90c9-2a8a-4b83-be9c-f988f54de29d;",
                   typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void SetupServiceLocator()
        {
            ServiceLocator.Add<ICloudService, AzureCloudService>();
        }
    }
}
