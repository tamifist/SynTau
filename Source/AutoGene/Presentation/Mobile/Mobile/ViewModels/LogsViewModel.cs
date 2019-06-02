using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using AutoGene.Mobile.Abstractions;
using AutoGene.Mobile.Enums;
using AutoGene.Mobile.Helpers;
using AutoGene.Mobile.Pages;
using AutoGene.Mobile.Services;
using Infrastructure.API.Synthesizer.Services;
using Newtonsoft.Json;
using Shared.DTO.Responses;
using Shared.DTO.Responses.Common;
using Shared.DTO.Responses.OligoSynthesizer;
using Shared.Enum;
using Shared.Resources;
using Xamarin.Forms;
using Shared.DTO.Responses.Diagnostic;
using Shared.DTO.Responses.GeneSynthesizer;

namespace AutoGene.Mobile.ViewModels
{
    public class LogsViewModel : BaseViewModel
    {
        private ValveRestService valveRestService;
        private OligoSynthesizerRestService oligoSynthesizerRestService;
        private SignalService signalService;

        private ICloudTable<Log> logTable;

        private Stopwatch timer;

        public LogsViewModel()
        {
            Title = AppResources.Logs_Title;
            
            ConnectCommand = new Command(async () => await Connect());
            //SettingsCommand = new Command(async () => await ShowSettings());
            SyncSettingsCommand = new Command(async () =>
            {
                Items.Insert(0, new Log() { Message = "Syncing settings..." });
                await SyncSettings();
            });

            // Subscribe to events from Settings Page
            MessagingCenter.Subscribe<SynthesizerSettingsViewModel>(this, "SynthesizerSettingsChanged", async (sender) =>
            {
                await SyncSettings();
            });

            MessagingCenter.Subscribe<SystemMonitorPage>(this, "TrayOut", async (sender) =>
            {
                await oligoSynthesizerRestService.ExecuteHardwareFunction("/macros/tray_out");
            });

            SyncSettingsCommand.Execute(null);
        }
        
        public ICommand ConnectCommand { get; }
        //public ICommand SettingsCommand { get; }
        public ICommand SyncSettingsCommand { get; }

        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();

        ObservableRangeCollection<Log> items = new ObservableRangeCollection<Log>();
        public ObservableRangeCollection<Log> Items
        {
            get { return items; }
            set { SetProperty(ref items, value, nameof(Items)); }
        }
        
        public SynthesizerSetting SynthesizerSettings { get; set; }

        public int SynthesisProcessTotalTime { get; set; }

        private async Task Connect()
        {
            try
            {
                logTable = await CloudService.GetTableAsync<Log>();
                ICollection<Log> logs = await logTable.ReadAllItemsAsync();

                Items.AddRange(logs.OrderByDescending(x => x.CreatedAt));

                if (SynthesizerSettings == null || string.IsNullOrWhiteSpace(SynthesizerSettings.AppServiceUrl) || 
                    string.IsNullOrWhiteSpace(SynthesizerSettings.SynthesizerApiUrl))
                {
                    LogAction("Settings are not configured");
                    return;
                }
                
                LogAction($"Connecting to {SynthesizerSettings.AppServiceUrl}");

                signalService = new SignalService(SynthesizerSettings.AppServiceUrl);
                signalService.MessageReceived += SignalMessageReceived;
                
                LogAction($"Connected to {SynthesizerSettings.AppServiceUrl}");

                string connectResponse = await signalService.Connect();
                LogAction(connectResponse);

                UserId = GetCurrentUserId();
                string joinGroupResponse = await signalService.JoinGroup(UserId);
                LogAction(joinGroupResponse);

                valveRestService = new ValveRestService(SynthesizerSettings.SynthesizerApiUrl);
                oligoSynthesizerRestService = new OligoSynthesizerRestService(SynthesizerSettings.SynthesizerApiUrl, LogAction);
            }
            catch (Exception ex)
            {
                LogAction($"Connection failed: {ex.Message}");
            }
        }
        
        private async Task ShowSettings()
        {
            if (SynthesizerSettings == null)
            {
                SynthesizerSettings = new SynthesizerSetting()
                {
                    AppServiceUrl = Identifiers.Environment.AppServiceUrl,
                    //SynthesizerApiUrl = Identifiers.Environment.SynthesizerApiUrl,
                    DelayAfterStrikeOn = 150
                };
            }

            await Application.Current.MainPage.Navigation.PushAsync(
                new SynthesizerSettingsPage(SynthesizerSettings));
        }

        private async Task SyncSettings()
        {
            if (IsOffline)
            {
                await UserDialogs.Instance.AlertAsync(AppResources.Logs_NoInternetConnection);
                return;
            }
            
            var synthesizerSettingTable = await CloudService.GetTableAsync<SynthesizerSetting>();
            var synthesizerSettings = await synthesizerSettingTable.ReadAllItemsAsync();
            if (synthesizerSettings != null && synthesizerSettings.Any())
            {
                SynthesizerSettings = synthesizerSettings.OrderByDescending(x => x.CreatedAt).First();

                //bool isSynthesizerApiUrlReachable = await IsLocalUrlReachable(SynthesizerSettings.SynthesizerApiUrl);
                //if (!isSynthesizerApiUrlReachable)
                //{
                //    await UserDialogs.Instance.AlertAsync(AppResources.Logs_NoSynthesizerConnection);
                //    return;
                //}

                MessagingCenter.Send<LogsViewModel>(this, "SynthesizerSettingsSynced");
                await Connect();
            }
            else
            {
                Items.Insert(0, new Log() { Message = "Synthesizer Settings not found." });
                //await ShowSettings();
            }

        }

        private async void SignalMessageReceived(object sender, SignalMessage signalMessage)
        {
            if (signalMessage.SignalType == SignalType.DeactivateAllValves)
            {
                await DeactivateAllValves(signalMessage.Message);
            }
            else if (signalMessage.SignalType == SignalType.ActivateValves)
            {
                await ActivateValves(signalMessage.Message);
            }
            else if (signalMessage.SignalType == SignalType.SyringePump)
            {
                try
                {
                    await oligoSynthesizerRestService.ExecuteHardwareFunction(signalMessage.Message);
                }
                catch (Exception ex)
                {
                }
                
            }
            // oligo synthesis
            else if (signalMessage.SignalType == SignalType.StartOligoSynthesisProcess)
            {
                if (oligoSynthesizerRestService.IsRunning && oligoSynthesizerRestService.IsSuspended)
                {
                    oligoSynthesizerRestService.ResumeSynthesis();
                }
                else
                {
                    await Task.Run(async () =>
                    {
                        await StartOligoSynthesisProcess();
                    });
                    
                }
            }
            else if (signalMessage.SignalType == SignalType.SuspendOligoSynthesisProcess)
            {
                oligoSynthesizerRestService.SuspendSynthesis();
            }
            else if (signalMessage.SignalType == SignalType.StopOligoSynthesisProcess)
            {
                oligoSynthesizerRestService.ForceStopSynthesis();
            }
            // gene synthesis
            else if (signalMessage.SignalType == SignalType.StartGeneSynthesisProcess)
            {
                if (oligoSynthesizerRestService.IsRunning && oligoSynthesizerRestService.IsSuspended)
                {
                    oligoSynthesizerRestService.ResumeSynthesis();
                }
                else
                {
                    await Task.Run(async () =>
                    {
                        await StartGeneSynthesisProcess();
                    });

                }
            }
            else if (signalMessage.SignalType == SignalType.SuspendGeneSynthesisProcess)
            {
                oligoSynthesizerRestService.SuspendSynthesis();
            }
            else if (signalMessage.SignalType == SignalType.StopGeneSynthesisProcess)
            {
                oligoSynthesizerRestService.ForceStopSynthesis();
            }
            else if (signalMessage.SignalType == SignalType.TrayOut || 
                signalMessage.SignalType == SignalType.TrayIn ||
                signalMessage.SignalType == SignalType.TrayLightOff ||
                signalMessage.SignalType == SignalType.TrayLightOn)
            {
                await ExecuteHardwareFunction(signalMessage.Message);
            }
            else if (signalMessage.SignalType == SignalType.GSValve)
            {
                await oligoSynthesizerRestService.ExecuteHardwareFunction(signalMessage.Message);
            }
        }

        private async Task StartOligoSynthesisProcess()
        {
            //try
            //{
            //    var oligoSynthesizerBackgroundService = DependencyService.Get<IOligoSynthesizerBackgroundService>();
            //    oligoSynthesizerBackgroundService.Start();
            //}
            //catch (Exception ex)
            //{
            //    LogAction($"Oligo synthesis process failed: {ex.Message}");
            //}

            ICloudTable<OligoSynthesisProcess> oligoSynthesisProcessTable = null;
            OligoSynthesisProcess currentOligoSynthesisProcess = null;

            try
            {
                await AwaitPreviousSynthesisProcess();

                LogAction("Syncing oligo synthesis process...");

                var currentUserId = GetCurrentUserId();

                LogAction("Syncing oligo synthesis process.");

                await CloudService.SyncOfflineCacheAsync();

                LogAction("Sync completed.");

                oligoSynthesisProcessTable = await CloudService.GetTableAsync<OligoSynthesisProcess>();
                var allOligoSynthesisProcesses = await oligoSynthesisProcessTable.ReadAllItemsAsync();
                currentOligoSynthesisProcess = allOligoSynthesisProcesses.Where(x => x.UserId == currentUserId)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefault();

                if (currentOligoSynthesisProcess == null)
                {
                    LogAction("Oligo synthesis process not found.");
                }

                var hardwareFunctionTable = await CloudService.GetTableAsync<HardwareFunction>();
                var allHardwareFunctions = await hardwareFunctionTable.ReadAllItemsAsync();
                IEnumerable<HardwareFunction> bAndTetToColFunctions = allHardwareFunctions.Where(
                    x => x.FunctionType == HardwareFunctionType.AAndTetToCol ||
                         x.FunctionType == HardwareFunctionType.CAndTetToCol ||
                         x.FunctionType == HardwareFunctionType.TAndTetToCol ||
                         x.FunctionType == HardwareFunctionType.GAndTetToCol);

                HardwareFunction closeAllValvesFunction =
                    allHardwareFunctions.Single(x => x.FunctionType == HardwareFunctionType.CloseAllValves);
                
                int oligoSynthesisProcessRemainingTime = currentOligoSynthesisProcess.TotalTime;

                LogAction($"Starting a new oligo synthesis process. Total Time: {oligoSynthesisProcessRemainingTime} s");

                foreach (OligoSynthesisActivity oligoSynthesisActivity in currentOligoSynthesisProcess.OligoSynthesisActivities.OrderBy(x => x.ChannelNumber))
                {
                    int oligoSynthesisActivityTotalTime = oligoSynthesisActivity.TotalTime;

                    await Task.Run(async () =>
                    {
                        await oligoSynthesizerRestService.StartSynthesis(oligoSynthesisActivity, bAndTetToColFunctions,
                            closeAllValvesFunction, oligoSynthesisActivityTotalTime);
                    });

                    oligoSynthesisProcessRemainingTime -= oligoSynthesisActivityTotalTime;
                    LogAction($"Total remaining time: {oligoSynthesisProcessRemainingTime} s");
                }

                //currentOligoSynthesisProcess.Status = SynthesisProcessStatus.Completed;
                //await oligoSynthesisProcessTable.UpdateItemAsync(currentOligoSynthesisProcess);
            }
            catch (Exception ex)
            {
                //currentOligoSynthesisProcess.Status = SynthesisProcessStatus.Failed;
                //await oligoSynthesisProcessTable.UpdateItemAsync(currentOligoSynthesisProcess);
                LogAction($"Oligo synthesis process failed: {ex.Message}");
            }
            finally
            {
                await CloudService.SyncOfflineCacheAsync();
            }
        }

        private async Task StartGeneSynthesisProcess()
        {
            ICloudTable<GeneSynthesisProcess> geneSynthesisProcessTable = null;
            GeneSynthesisProcess currentGeneSynthesisProcess = null;

            try
            {
                await AwaitPreviousSynthesisProcess();

                LogAction("Syncing gene synthesis process...");

                var currentUserId = GetCurrentUserId();

                await CloudService.SyncOfflineCacheAsync();

                LogAction("Sync completed.");

                geneSynthesisProcessTable = await CloudService.GetTableAsync<GeneSynthesisProcess>();
                var allGeneSynthesisProcesses = await geneSynthesisProcessTable.ReadAllItemsAsync();
                currentGeneSynthesisProcess = allGeneSynthesisProcesses.Where(x => x.Gene.UserId == currentUserId)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefault();

                if (currentGeneSynthesisProcess == null)
                {
                    LogAction("Gene synthesis process not found.");
                }

                var hardwareFunctionTable = await CloudService.GetTableAsync<HardwareFunction>();
                var allHardwareFunctions = await hardwareFunctionTable.ReadAllItemsAsync();

                IEnumerable<HardwareFunction> bAndTetToColFunctions = allHardwareFunctions.Where(
                    x => x.FunctionType == HardwareFunctionType.AAndTetToCol ||
                         x.FunctionType == HardwareFunctionType.CAndTetToCol ||
                         x.FunctionType == HardwareFunctionType.TAndTetToCol ||
                         x.FunctionType == HardwareFunctionType.GAndTetToCol);

                HardwareFunction closeAllValvesFunction =
                    allHardwareFunctions.Single(x => x.FunctionType == HardwareFunctionType.CloseAllValves);

                SynthesisProcessTotalTime = currentGeneSynthesisProcess.TotalTime;

                MessagingCenter.Send<LogsViewModel>(this, "SynthesisProcessStarted");

                int geneSynthesisProcessRemainingTime = currentGeneSynthesisProcess.TotalTime;

                LogAction($"Starting a new gene synthesis process. Total Time: {geneSynthesisProcessRemainingTime} s");

                foreach (GeneSynthesisActivity geneSynthesisActivity in currentGeneSynthesisProcess.GeneSynthesisActivities.OrderBy(x => x.ChannelNumber))
                {
                    int oligoSynthesisActivityTotalTime = geneSynthesisActivity.TotalTime;

                    await Task.Run(async () =>
                    {
                        await oligoSynthesizerRestService.StartSynthesis(geneSynthesisActivity, bAndTetToColFunctions,
                            closeAllValvesFunction, oligoSynthesisActivityTotalTime);
                    });

                    geneSynthesisProcessRemainingTime -= oligoSynthesisActivityTotalTime;
                    LogAction($"Total remaining time: {geneSynthesisProcessRemainingTime} s");
                }

                //currentOligoSynthesisProcess.Status = SynthesisProcessStatus.Completed;
                //await oligoSynthesisProcessTable.UpdateItemAsync(currentOligoSynthesisProcess);
            }
            catch (Exception ex)
            {
                //currentOligoSynthesisProcess.Status = SynthesisProcessStatus.Failed;
                //await oligoSynthesisProcessTable.UpdateItemAsync(currentOligoSynthesisProcess);
                LogAction($"Gene synthesis process failed: {ex.Message}");
            }
            finally
            {
                await CloudService.SyncOfflineCacheAsync();
            }
        }

        private string GetCurrentUserId()
        {
            var userId = Settings.GetSetting<string>(AppSettings.UserId);
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("UserId is not set.");
            }
            else
            {
                LogAction($"UserId: {userId}");
            }

            return userId;
        }

        private async Task AwaitPreviousSynthesisProcess()
        {
            if (oligoSynthesizerRestService.IsRunning)
            {
                LogAction("Waiting for the previous synthesis process.");

                while (oligoSynthesizerRestService.IsRunning)
                {
                    await Task.Delay(100);
                }

                LogAction("Synthesis process has been completed.");
            }
        }

        private async Task ActivateValves(string signalMessage)
        {
            IEnumerable<HardwareFunctionItem> hardwareFunctions =
                JsonConvert.DeserializeObject<IEnumerable<HardwareFunctionItem>>(signalMessage);
            IEnumerable<string> valvesNames = hardwareFunctions.Where(x => x.FunctionType == HardwareFunctionType.Valve).Select(x => x.Name);

            LogAction($"Activating valves(delay after uStrkOn set to {SynthesizerSettings.DelayAfterStrikeOn} ms): {string.Join(", ", valvesNames)}");

            timer = Stopwatch.StartNew();

            string responseMessage = await valveRestService.ActivateValves(hardwareFunctions, SynthesizerSettings.DelayAfterStrikeOn);

            LogAction(responseMessage);
        }

        private async Task DeactivateAllValves(string signalMessage)
        {
            HardwareFunctionItem deactivateAllValvesHardwareFunction = JsonConvert.DeserializeObject<HardwareFunctionItem>(signalMessage);
            string responseMessage = await valveRestService.DeactivateAllValves(deactivateAllValvesHardwareFunction);

            timer?.Stop();

            LogAction(responseMessage);
            LogAction($"DeactivateAllValves: Executed in {timer?.ElapsedMilliseconds} ms");
        }

        private async Task ExecuteHardwareFunction(string signalMessage)
        {
            HardwareFunctionItem hardwareFunction = JsonConvert.DeserializeObject<HardwareFunctionItem>(signalMessage);
            await oligoSynthesizerRestService.ExecuteHardwareFunction(hardwareFunction.ApiUrl);
        }

        private void LogAction(string msg)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var log = new Log() { Message = msg };
                Items.Insert(0, log);

                await logTable.UpsertItemAsync(log);
            });
        }
    }
}