using Shared.DTO.Responses;
using Shared.Resources;
using Xamarin.Forms;

namespace AutoGene.Mobile.Pages
{
    public class RootPage : TabbedPage
    {
        public RootPage()
        {
            var systemMonitorPage = new SystemMonitorPage();
            var mainPage = new LogsPage();
            var synthesizerSettingsPage = new SynthesizerSettingsPage(new SynthesizerSetting()
            {
                AppServiceUrl = Identifiers.Environment.AppServiceUrl,
                //SynthesizerApiUrl = Identifiers.Environment.SynthesizerApiUrl,
                DelayAfterStrikeOn = 150
            });

            Children.Add(systemMonitorPage);
            Children.Add(mainPage);
            Children.Add(synthesizerSettingsPage);

            SelectedItem = systemMonitorPage;

            Title = systemMonitorPage.Title;
        }
    }
}
