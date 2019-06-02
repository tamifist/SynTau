using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoGene.Mobile.ViewModels;
using Shared.DTO.Responses;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoGene.Mobile.Pages
{
    public partial class SynthesizerSettingsPage : ContentPage
    {
        public SynthesizerSettingsPage(SynthesizerSetting synthesizerSettings)
        {
            InitializeComponent();
            BindingContext = new SynthesizerSettingsViewModel(synthesizerSettings);
        }
    }
}
