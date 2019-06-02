using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Shared.DTO.Responses;
using Shared.Enum;
using Shared.Resources;

namespace Infrastructure.API.Synthesizer.Services
{
    public class ValveRestService : RestService
    {
        public ValveRestService(string synthesizerApiUrl) : base(synthesizerApiUrl)
		{
        }

        public async Task<string> ActivateValves(IEnumerable<HardwareFunctionItem> hardwareFunctions, int delayAfterStrikeOn)
        {
            try
            {
                IList<HardwareFunctionItem> valvesHardwareFunctions = 
                    hardwareFunctions.Where(x => x.FunctionType == HardwareFunctionType.Valve).OrderBy(x => x.Number).ToList();
                foreach (HardwareFunctionItem valveHardwareFunction in valvesHardwareFunctions)
                {
                    string valveUrl = BaseUrl + valveHardwareFunction.ApiUrl;
                    HttpResponseMessage valveResponse = await Client.PostAsync(valveUrl, null);
                    if (!valveResponse.IsSuccessStatusCode)
                    {
                        return $"Error: {valveUrl}. Message: {valveResponse.ToString()}";
                    }
                }

                HardwareFunctionItem strikeOnHardwareFunction = hardwareFunctions.Single(x => x.FunctionType == HardwareFunctionType.StrikeOn);
                string uStrkOnUrl = BaseUrl + strikeOnHardwareFunction.ApiUrl;
                HttpResponseMessage strikeOnResponse = await Client.PostAsync(uStrkOnUrl, null);
                if (!strikeOnResponse.IsSuccessStatusCode)
                {
                    return $"Error: {uStrkOnUrl}. Message: {strikeOnResponse.ToString()}";
                }

                await Task.Delay(delayAfterStrikeOn);

                HardwareFunctionItem holdOnHardwareFunction = hardwareFunctions.Single(x => x.FunctionType == HardwareFunctionType.HoldOn);
                string uHoldOnUrl = BaseUrl + holdOnHardwareFunction.ApiUrl;
                HttpResponseMessage uHoldOnResponse = await Client.PostAsync(uHoldOnUrl, null);
                if (!uHoldOnResponse.IsSuccessStatusCode)
                {
                    return $"Error: {uHoldOnUrl}. Message: {uHoldOnResponse.ToString()}";
                }

                HardwareFunctionItem strikeOffHardwareFunction = hardwareFunctions.Single(x => x.FunctionType == HardwareFunctionType.StrikeOff);
                string uStrkOffUrl = BaseUrl + strikeOffHardwareFunction.ApiUrl;
                HttpResponseMessage uStrkOffResponse = await Client.PostAsync(uStrkOffUrl, null);
                if (!uStrkOffResponse.IsSuccessStatusCode)
                {
                    return $"Error: {uStrkOffUrl}. Message: {uStrkOffResponse.ToString()}";
                }

                return "Valves activated";
            }
            catch (Exception ex)
            {
                return $"Failed to activate valves. Message: {ex.Message}";
            }
        }

        public async Task<string> DeactivateAllValves(HardwareFunctionItem deactivateAllValvesHardwareFunction)
        {
            try
            {
                string deactivateAllValvesUrl = BaseUrl + deactivateAllValvesHardwareFunction.ApiUrl;
                HttpResponseMessage deactivateAllValvesResponse = await Client.PostAsync(deactivateAllValvesUrl, null);
                if (!deactivateAllValvesResponse.IsSuccessStatusCode)
                {
                    return $"Error: {deactivateAllValvesUrl}. Message: {deactivateAllValvesResponse.ToString()}";
                }

                return AppResources.Infrastructure_AllValvesDeactivatedMsg;
            }
            catch (Exception ex)
            {
                return $"DeactivateAllValves failed. Message: {ex.Message}";
            }
        }
    }
}