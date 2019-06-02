using AutoGene.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoGene.Mobile.Pages
{
    public partial class LogsPage : ContentPage
    {
        public LogsPage()
        {
            InitializeComponent();
            BindingContext = new LogsViewModel();
        }
    }
}
