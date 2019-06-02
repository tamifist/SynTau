using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using AutoGene.Mobile.Abstractions;
using AutoGene.Mobile.Helpers;
using Shared.DTO.Responses;
using Shared.Resources;
using Xamarin.Forms;

namespace AutoGene.Mobile.ViewModels
{
    public class SynthesizerSettingsViewModel : BaseViewModel
    {
        public SynthesizerSettingsViewModel(SynthesizerSetting synthesizerSettings)
        {
            Title = AppResources.SynthesizerSettings_Title;

            Model = synthesizerSettings;

            SaveCommand = new Command(async () => await Save());
            DeleteCommand = new Command(async () => await Delete());

            MessagingCenter.Subscribe<LogsViewModel>(this, "SynthesizerSettingsSynced", (sender) =>
            {
                Model = sender.SynthesizerSettings;
            });
        }

        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }

        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();

        SynthesizerSetting model;
        public SynthesizerSetting Model
        {
            get
            {
                return model;
            }
            set
            {
                SetProperty(ref model, value, "Model");
            }
        }

        async Task Save()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            UserDialogs.Instance.ShowLoading("Applying settings");

            try
            {
                var table = await CloudService.GetTableAsync<SynthesizerSetting>();
                await table.UpsertItemAsync(Model);
                await CloudService.SyncOfflineCacheAsync();
                MessagingCenter.Send<SynthesizerSettingsViewModel>(this, "SynthesizerSettingsChanged");
                //await Application.Current.MainPage.Navigation.PopAsync();
                //var allItems = await table.ReadAllItemsAsync();
                //MessagingCenter.Send<TagDetailViewModel>(this, "ItemsChanged");
                //await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.SynthesizerSettings_SaveSettingsFailedMsg,
                    ex.Message, AppResources.OK);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                IsBusy = false;
            }
        }

        async Task Delete()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                if (Model.Id != null)
                {
                    var table = await CloudService.GetTableAsync<SynthesizerSetting>();
                    await table.DeleteItemAsync(Model);
                    await CloudService.SyncOfflineCacheAsync();
                    //MessagingCenter.Send<TagDetailViewModel>(this, "ItemsChanged");
                }
                //await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Delete Item Failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}