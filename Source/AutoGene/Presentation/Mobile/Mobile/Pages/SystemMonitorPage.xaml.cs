using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using AutoGene.Mobile.ViewModels;
using Shared.Resources;
using Telerik.XamarinForms.DataVisualization.Gauges;
using Xamarin.Forms;

namespace AutoGene.Mobile.Pages
{
    public partial class SystemMonitorPage : ContentPage
    {
        private Random _random;
        public SystemMonitorPage()
        {
            InitializeComponent();

            Title = AppResources.SystemMonitor_Title;
            _random = new Random();

            MessagingCenter.Subscribe<LogsViewModel>(this, "SynthesisProcessStarted", (sender) =>
            {
                SetProgressBar(sender.SynthesisProcessTotalTime);
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateManometers();
        }

        private async void UpdateManometers()
        {
            GaugeNeedleIndicator manometer1Indicator = (GaugeNeedleIndicator)this.manometer1.Indicators[0];
            GaugeNeedleIndicator manometer2Indicator = (GaugeNeedleIndicator)this.manometer2.Indicators[0];
            GaugeNeedleIndicator manometer3Indicator = (GaugeNeedleIndicator)this.manometer3.Indicators[0];
            GaugeNeedleIndicator manometer4Indicator = (GaugeNeedleIndicator)this.manometer4.Indicators[0];
            while (true)
            {
                await Task.Delay(250);
                manometer1Indicator.Value = _random.Next(20, 40);

                await Task.Delay(250);
                manometer2Indicator.Value = _random.Next(80, 100);

                await Task.Delay(250);
                manometer3Indicator.Value = _random.Next(120, 140);

                await Task.Delay(250);
                manometer4Indicator.Value = _random.Next(160, 180);
            }
        }

        private async void SetProgressBar(int totalTime)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    progressBar.IsVisible = true;
                });
                
                double countOfSteps = 20.0;
                int stepTime = (int)Math.Round(totalTime / countOfSteps, MidpointRounding.AwayFromZero);

                GaugeLinearAxis gaugeLinearAxis = new GaugeLinearAxis();
                gaugeLinearAxis.Minimum = 0;
                gaugeLinearAxis.Maximum = totalTime;
                gaugeLinearAxis.Step = stepTime;
                this.progressBar.Axis = gaugeLinearAxis;

                GaugeRange progressBarRange = (GaugeRange)this.progressBar.Ranges.Ranges[0];
                while (progressBarRange.To < totalTime)
                {
                    progressBarRange.To += stepTime;
                    await Task.Delay(stepTime * 1000);
                }
                
                await UserDialogs.Instance.AlertAsync("Synthesis completed");

                //MessagingCenter.Send<SystemMonitorPage>(this, "TrayOut");

                Device.BeginInvokeOnMainThread(() =>
                {
                    progressBarRange.To = 0;
                    progressBar.IsVisible = false;
                });
            }
            catch (Exception ex)
            {
            }
        }
    }
}
