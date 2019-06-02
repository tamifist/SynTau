//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using AutoGene.Mobile.Abstractions;
//using AutoGene.Mobile.Enums;
//using AutoGene.Mobile.Helpers;
//using Infrastructure.API.Synthesizer.Services;
//using Shared.DTO.Responses.Common;
//using Shared.DTO.Responses.Diagnostic;
//using Shared.DTO.Responses.OligoSynthesizer;
//using Shared.Enum;
//using Shared.Resources;
//using Microsoft.WindowsAzure.MobileServices;

//namespace AutoGene.Mobile.Services
//{
//    public class OligoSynthesizerService
//    {
//        private readonly OligoSynthesizerRestService oligoSynthesizerRestService;
//        private ICloudTable<Log> logTable;

//        public OligoSynthesizerService()
//        {
//            oligoSynthesizerRestService = new OligoSynthesizerRestService(Identifiers.Environment.AppServiceUrl, LogAction);
//        }

//        public ICloudService CloudService => ServiceLocator.Get<ICloudService>();

//        public bool IsRunning => oligoSynthesizerRestService.IsRunning;

//        public bool IsSuspended => oligoSynthesizerRestService.IsSuspended;

//        public bool IsForceStopRequested => oligoSynthesizerRestService.IsForceStopRequested;

//        public void SuspendSynthesis()
//        {
//            oligoSynthesizerRestService.SuspendSynthesis();
//        }

//        public void ResumeSynthesis()
//        {
//            oligoSynthesizerRestService.ResumeSynthesis();
//        }

//        public void ForceStopSynthesis()
//        {
//            oligoSynthesizerRestService.ForceStopSynthesis();
//        }

//        public async Task StartOligoSynthesisProcess()
//        {
//            try
//            {
//                logTable = await CloudService.GetTableAsync<Log>();

//                if (oligoSynthesizerRestService.IsRunning)
//                {
//                    LogAction("Waiting for the previous oligo synthesis process.");

//                    while (oligoSynthesizerRestService.IsRunning)
//                    {
//                        await Task.Delay(100);
//                    }

//                    LogAction("Oligo synthesis process has been completed.");
//                }

//                LogAction("Syncing oligo synthesis process...");

//                var userId = Settings.GetSetting<string>(AppSettings.UserId);
//                if (string.IsNullOrEmpty(userId))
//                {
//                    throw new Exception("UserId is not set.");
//                }
//                else
//                {
//                    LogAction($"UserId: {userId}");
//                }

//                LogAction("Syncing oligo synthesis process.");

//                await CloudService.SyncOfflineCacheAsync();

//                LogAction("Sync completed.");

//                var oligoSynthesisProcessTable = await CloudService.GetTableAsync<OligoSynthesisProcess>();
//                var allOligoSynthesisProcesses = await oligoSynthesisProcessTable.ReadAllItemsAsync();
//                OligoSynthesisProcess currentOligoSynthesisProcess = allOligoSynthesisProcesses.Where(x => x.UserId == userId)
//                    .OrderByDescending(x => x.CreatedAt)
//                    .FirstOrDefault();

//                if (currentOligoSynthesisProcess == null)
//                {
//                    LogAction("Oligo synthesis process not found.");
//                }

//                var hardwareFunctionTable = await CloudService.GetTableAsync<HardwareFunction>();
//                var allHardwareFunctions = await hardwareFunctionTable.ReadAllItemsAsync();
//                IEnumerable<HardwareFunction> bAndTetToColFunctions = allHardwareFunctions.Where(
//                    x => x.FunctionType == HardwareFunctionType.AAndTetToCol ||
//                         x.FunctionType == HardwareFunctionType.CAndTetToCol ||
//                         x.FunctionType == HardwareFunctionType.TAndTetToCol ||
//                         x.FunctionType == HardwareFunctionType.GAndTetToCol);

//                HardwareFunction closeAllValvesFunction =
//                    allHardwareFunctions.Single(x => x.FunctionType == HardwareFunctionType.CloseAllValves);

//                CalcOligoSynthesisProcessTotalTime(currentOligoSynthesisProcess);
//                int oligoSynthesisProcessRemainingTime = currentOligoSynthesisProcess.TotalTime;

//                LogAction($"Starting a new oligo synthesis process. Total Time: {oligoSynthesisProcessRemainingTime} s");

//                foreach (OligoSynthesisActivity oligoSynthesisActivity in currentOligoSynthesisProcess.OligoSynthesisActivities.OrderBy(x => x.ChannelNumber))
//                {
//                    int oligoSynthesisActivityTotalTime = oligoSynthesisActivity.TotalTime;

//                    await oligoSynthesizerRestService.StartSynthesis(oligoSynthesisActivity, bAndTetToColFunctions,
//                        closeAllValvesFunction, oligoSynthesisActivityTotalTime);

//                    oligoSynthesisProcessRemainingTime -= oligoSynthesisActivityTotalTime;
//                    LogAction($"Total remaining time: {oligoSynthesisProcessRemainingTime} s");
//                }
//            }
//            catch (Exception ex)
//            {
//                LogAction($"Oligo synthesis process failed: {ex.Message}");
//            }
//            finally
//            {
//                await CloudService.SyncOfflineCacheAsync();
//            }
//        }

//        private void CalcOligoSynthesisProcessTotalTime(OligoSynthesisProcess oligoSynthesisProcess)
//        {

//            int oligoSynthesisProcessTotalTime = 0;
//            foreach (OligoSynthesisActivity oligoSynthesisActivity in oligoSynthesisProcess.OligoSynthesisActivities)
//            {
//                CalcOligoSynthesisActivityTotalTime(oligoSynthesisActivity);
//                oligoSynthesisProcessTotalTime += oligoSynthesisActivity.TotalTime;
//            }

//            oligoSynthesisProcess.TotalTime = oligoSynthesisProcessTotalTime;
//        }

//        private void CalcOligoSynthesisActivityTotalTime(OligoSynthesisActivity oligoSynthesisActivity)
//        {
//            string dnaSequence =
//                oligoSynthesisActivity.DNASequence.Substring(1, oligoSynthesisActivity.DNASequence.Length - 1);

//            int synthesisActivityTime = 0;
//            foreach (char nucleotide in dnaSequence.ToCharArray())
//            {
//                foreach (CycleStep cycleStep in oligoSynthesisActivity.SynthesisCycle.CycleSteps.OrderBy(x => x.Number))
//                {
//                    bool isCycleStepApplicable = oligoSynthesizerRestService.IsCycleStepApplicable(nucleotide, cycleStep);
//                    if (isCycleStepApplicable)
//                    {
//                        synthesisActivityTime += cycleStep.StepTime;
//                    }
//                }
//            }

//            oligoSynthesisActivity.TotalTime = synthesisActivityTime;
//        }

//        private async void LogAction(string msg)
//        {
//            var log = new Log() { Message = msg };
//            await logTable.UpsertItemAsync(log);
//            await CloudService.Client.SyncContext.PushAsync();
//        }
//    }
//}