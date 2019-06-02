using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Acr.UserDialogs;
using AutoGene.Mobile.Abstractions;
using AutoGene.Mobile.Enums;
using AutoGene.Mobile.Helpers;
using AutoGene.Mobile.Pages;
using Shared.DTO.Responses.Identity;
using Shared.Resources;
using Xamarin.Forms;

namespace AutoGene.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            Title = AppResources.SynthesizerSettings_Title;

            LogInCommand = new Command(() => LogIn());
        }

        public string Email { get; set; }

        public string Password { get; set; }

        public Command LogInCommand { get; }

        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();

        public async Task LogIn()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();

                string loggedInUserId = await CloudService.Client.InvokeApiAsync<string>("CustomAuth", HttpMethod.Get,
                    new Dictionary<string, string>
                    {
                        {"email", Email},
                        {"password", Password}
                    });

                if (string.IsNullOrEmpty(loggedInUserId))
                {
                    await Application.Current.MainPage.DisplayAlert(string.Empty, AppResources.Login_UserNotFound,
                        AppResources.OK);
                    UserDialogs.Instance.HideLoading();
                    return;
                }

                Settings.SetSetting(AppSettings.UserId, loggedInUserId);

                //var userId = Settings.GetSetting<string>(AppSettings.UserId);

                UserDialogs.Instance.HideLoading();

                Application.Current.MainPage = new RootPage();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(string.Empty, AppResources.Login_ConnectivityError,
                    AppResources.OK);
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}