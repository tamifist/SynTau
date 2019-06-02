using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Shared.DTO.Responses.Common;
using Shared.DTO.Responses.GeneSynthesizer;
using Shared.DTO.Responses.OligoSynthesizer;
using Shared.Enum;

namespace Infrastructure.API.Synthesizer.Services
{
    public class OligoSynthesizerRestService : RestService
    {
        private readonly Action<string> logAction;

        private bool isRunning;
        private bool isForceStopRequested;
        private bool isSuspended;

        public OligoSynthesizerRestService(string synthesizerApiUrl, Action<string> logAction) : base(synthesizerApiUrl)
        {
            this.logAction = logAction;
            ResetStates();
        }

        public bool IsRunning
        {
            get { return isRunning; }
        }

        public bool IsSuspended
        {
            get { return isSuspended; }
        }

        public bool IsForceStopRequested
        {
            get { return isForceStopRequested; }
        }

        public void SuspendSynthesis()
        {
            isSuspended = true;
        }

        public void ResumeSynthesis()
        {
            isSuspended = false;
        }

        public void ForceStopSynthesis()
        {
            logAction("Force Stop requested. Synthesis will continue execution to the next safe step.");
            isForceStopRequested = true;
            isSuspended = false;
        }

        public async Task StartSynthesis(OligoSynthesisActivity oligoSynthesisActivity, IEnumerable<HardwareFunction> bAndTetToColFunctions,
            HardwareFunction closeAllValvesFunction, int oligoSynthesisActivityTotalTime)
        {
            try
            {
                isRunning = true;

                if (oligoSynthesisActivity.DNASequence.Length < 2)
                {
                    throw new Exception($"DNA sequence length should be >= 2: {oligoSynthesisActivity.DNASequence}");
                }

                logAction(
                    $"Starting oligo synthesis activity. CH '{oligoSynthesisActivity.ChannelNumber}': SEQ '{oligoSynthesisActivity.DNASequence}' : Time '{oligoSynthesisActivityTotalTime} s'");

                string dnaSequence =
                    oligoSynthesisActivity.DNASequence.Substring(1, oligoSynthesisActivity.DNASequence.Length - 1);

                await ExecuteHardwareFunction(oligoSynthesisActivity.ChannelApiFunction.ApiUrl);
                
                foreach (char nucleotide in dnaSequence.ToCharArray())
                {
                    foreach (CycleStep cycleStep in oligoSynthesisActivity.SynthesisCycle.CycleSteps.OrderBy(x => x.Number))
                    {
                        bool isCycleStepApplicable = IsCycleStepApplicable(nucleotide, cycleStep);

                        if (isCycleStepApplicable)
                        {
                            string cycleStepApiUrl = GetCycleStepApiUrl(cycleStep, nucleotide, bAndTetToColFunctions);

                            logAction(
                                $"CH '{oligoSynthesisActivity.ChannelNumber}': SEQ '{oligoSynthesisActivity.DNASequence}': NUC '{nucleotide}' : STEP '{cycleStep.Number}' : TIME: {cycleStep.StepTime} : FUNC '{cycleStepApiUrl}': NAME '{cycleStep.HardwareFunction.Name}'");

                            await ExecuteHardwareFunction(cycleStepApiUrl);

                            await Task.Delay(cycleStep.StepTime * 1000);

                            logAction($"Opened valves: {cycleStep.HardwareFunction.Description}");

                            await ExecuteHardwareFunction(closeAllValvesFunction.ApiUrl);

                            oligoSynthesisActivityTotalTime -= cycleStep.StepTime;
                            logAction($"Remaining time: {oligoSynthesisActivityTotalTime} s ");

                            if (cycleStep.SafeStep && IsSuspended)
                            {
                                logAction("Synthesis suspended");

                                while (isSuspended)
                                {
                                    await Task.Delay(100);
                                }

                                logAction("Synthesis resumed");
                            }

                            if (cycleStep.SafeStep && IsForceStopRequested)
                            {
                                ResetStates();
                                logAction("Synthesis stopped");
                                return;
                            }
                        }
                    }
                }

                logAction(
                    $"Synthesis completed. CH '{oligoSynthesisActivity.ChannelNumber}': SEQ '{oligoSynthesisActivity.DNASequence}'");
            }
            catch (Exception ex)
            {
                logAction(
                    $"Synthesis failed: CH '{oligoSynthesisActivity.ChannelNumber}': SEQ '{oligoSynthesisActivity.DNASequence}'. Message: {ex.Message}");
                await ExecuteHardwareFunction(closeAllValvesFunction.ApiUrl);
            }
            finally
            {
                ResetStates();
            }
        }

        public async Task StartSynthesis(GeneSynthesisActivity geneSynthesisActivity, IEnumerable<HardwareFunction> bAndTetToColFunctions,
            HardwareFunction closeAllValvesFunction, int geneSynthesisActivityTotalTime)
        {
            try
            {
                isRunning = true;

                if (geneSynthesisActivity.DNASequence.Length < 2)
                {
                    throw new Exception($"DNA sequence length should be >= 2: {geneSynthesisActivity.DNASequence}");
                }

                logAction(
                    $"Starting gene synthesis activity. CH '{geneSynthesisActivity.ChannelNumber}': SEQ '{geneSynthesisActivity.DNASequence}' : Time '{geneSynthesisActivityTotalTime} s'");

                string dnaSequence =
                    geneSynthesisActivity.DNASequence.Substring(1, geneSynthesisActivity.DNASequence.Length - 1);

                await ExecuteHardwareFunction(geneSynthesisActivity.ChannelApiFunction.ApiUrl);

                foreach (char nucleotide in dnaSequence.ToCharArray())
                {
                    foreach (CycleStep cycleStep in geneSynthesisActivity.SynthesisCycle.CycleSteps.OrderBy(x => x.Number))
                    {
                        bool isCycleStepApplicable = IsCycleStepApplicable(nucleotide, cycleStep);

                        if (isCycleStepApplicable)
                        {
                            string cycleStepApiUrl = GetCycleStepApiUrl(cycleStep, nucleotide, bAndTetToColFunctions);

                            logAction(
                                $"CH '{geneSynthesisActivity.ChannelNumber}': SEQ '{geneSynthesisActivity.DNASequence}': NUC '{nucleotide}' : STEP '{cycleStep.Number}' : TIME: {cycleStep.StepTime} : FUNC '{cycleStepApiUrl}': NAME '{cycleStep.HardwareFunction.Name}'");

                            await ExecuteHardwareFunction(cycleStepApiUrl);

                            await Task.Delay(cycleStep.StepTime * 1000);

                            logAction($"Opened valves: {cycleStep.HardwareFunction.Description}");

                            await ExecuteHardwareFunction(closeAllValvesFunction.ApiUrl);

                            geneSynthesisActivityTotalTime -= cycleStep.StepTime;
                            logAction($"Remaining time: {geneSynthesisActivityTotalTime} s ");

                            if (cycleStep.SafeStep && IsSuspended)
                            {
                                logAction("Synthesis suspended");

                                while (isSuspended)
                                {
                                    await Task.Delay(100);
                                }

                                logAction("Synthesis resumed");
                            }

                            if (cycleStep.SafeStep && IsForceStopRequested)
                            {
                                ResetStates();
                                logAction("Synthesis stopped");
                                return;
                            }
                        }
                    }
                }

                logAction(
                    $"Synthesis completed. CH '{geneSynthesisActivity.ChannelNumber}': SEQ '{geneSynthesisActivity.DNASequence}'");
            }
            catch (Exception ex)
            {
                logAction(
                    $"Synthesis failed: CH '{geneSynthesisActivity.ChannelNumber}': SEQ '{geneSynthesisActivity.DNASequence}'. Message: {ex.Message}");
                await ExecuteHardwareFunction(closeAllValvesFunction.ApiUrl);
            }
            finally
            {
                ResetStates();
            }
        }

        public bool IsCycleStepApplicable(char nucleotide, CycleStep cycleStep)
        {
            bool isCycleStepApplicable = (nucleotide == 'A' && cycleStep.A) ||
                                         (nucleotide == 'C' && cycleStep.C) ||
                                         (nucleotide == 'G' && cycleStep.G) ||
                                         (nucleotide == 'T' && cycleStep.T);

            return isCycleStepApplicable;
        }

        private string GetCycleStepApiUrl(CycleStep cycleStep, char nucleotide, IEnumerable<HardwareFunction> bAndTetToColFunctions)
        {
            string cycleStepApiUrl = cycleStep.HardwareFunction.ApiUrl;

            if (cycleStep.HardwareFunction.FunctionType == HardwareFunctionType.BAndTetToCol)
            {
                switch (nucleotide)
                {
                    case 'A':
                        cycleStepApiUrl = bAndTetToColFunctions
                            .Single(x => x.FunctionType == HardwareFunctionType.AAndTetToCol)
                            .ApiUrl;
                        break;
                    case 'C':
                        cycleStepApiUrl = bAndTetToColFunctions
                            .Single(x => x.FunctionType == HardwareFunctionType.CAndTetToCol)
                            .ApiUrl;
                        break;
                    case 'T':
                        cycleStepApiUrl = bAndTetToColFunctions
                            .Single(x => x.FunctionType == HardwareFunctionType.TAndTetToCol)
                            .ApiUrl;
                        break;
                    case 'G':
                        cycleStepApiUrl = bAndTetToColFunctions
                            .Single(x => x.FunctionType == HardwareFunctionType.GAndTetToCol)
                            .ApiUrl;
                        break;
                }
            }

            return cycleStepApiUrl;
        }

        public async Task ExecuteHardwareFunction(string apiUrl, int retryCount = 10)
        {
            string hardwareFunctionApiUrl = BaseUrl + apiUrl;

            try
            {
                bool isSuccessStatusCode = true;
                int retry = 0;

                do
                {
                    retry++;
                    
                    logAction($"PostAsync: {hardwareFunctionApiUrl}");

                    HttpResponseMessage hardwareFunctionResponse = await Client.PostAsync(hardwareFunctionApiUrl, null);

                    isSuccessStatusCode = hardwareFunctionResponse.IsSuccessStatusCode;

                    if (!isSuccessStatusCode)
                    {
                        if (retry < retryCount)
                        {
                            logAction($"Retry: {hardwareFunctionApiUrl}");
                            await Task.Delay(2000);
                        }
                        else
                        {
                            throw new Exception($"API URL: {hardwareFunctionApiUrl}. Error message: {hardwareFunctionResponse.ToString()}");
                        }
                    }

                } while (!isSuccessStatusCode && retry < retryCount);
            }
            catch (Exception ex)
            {
                logAction($"Failed to execute {hardwareFunctionApiUrl}. Error: {ex.Message}");
                throw;
            }
        }
        
        private void ResetStates()
        {
            isRunning = false;
            isSuspended = false;
            isForceStopRequested = false;
        }
    }
}