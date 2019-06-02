using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoGene.Presentation.Host.Hubs;
using Business.Contracts.Services.ManualControl;
using Business.Contracts.ViewModels.Common;
using Business.Contracts.ViewModels.ManualControl;
using Data.Contracts;
using Data.Contracts.Entities.CycleEditor;
using Data.Contracts.Entities.Identity;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Presentation.Common.Controllers;
using Presentation.Common.Security;
using Shared.DTO.Responses;
using Shared.Enum;
using Shared.Framework.Collections;
using Shared.Resources;

namespace AutoGene.Presentation.Host.Controllers.ManualControl
{
    [AuthorizeUser(UserRoles = "Guest,Admin")]
    public class ManualControlController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IManualControlService manualControlService;
        private readonly IHubContext signalHubContext;

        public ManualControlController(IUnitOfWork unitOfWork, IManualControlService manualControlService)
        {
            this.unitOfWork = unitOfWork;
            this.manualControlService = manualControlService;
            signalHubContext = GlobalHost.ConnectionManager.GetHubContext<SignalHub>();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            ManualControlViewModel manualControlViewModel = await manualControlService.GetManualControlViewModel();
            return View(manualControlViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> ActivateValves(IEnumerable<HardwareFunctionItemViewModel> selectedValves)
        {
            if (selectedValves != null && selectedValves.Any())
            {
                manualControlService.ActivateValves(selectedValves);

                var hardwareFunctions = (await manualControlService.GetBasicHardwareFunctions()).ToList();

                hardwareFunctions.AddRange(selectedValves);

                User currentUser = manualControlService.GetCurrentUser();

                await signalHubContext.Clients.Group(currentUser.Id).addMessage(
                    new SignalMessage
                    {
                        Message = JsonConvert.SerializeObject(hardwareFunctions),
                        SignalType = SignalType.ActivateValves
                    });

                return JsonSuccess(string.Empty);
            }

            return JsonError();
        }

        [HttpPost]
        public async Task<JsonResult> DeactivateAllValves()
        {
            await manualControlService.DeactivateAllValves();

            await ExecuteHardwareFunction(HardwareFunctionType.CloseAllValves, SignalType.DeactivateAllValves);
            
            return JsonSuccess(string.Empty);
        }

        [HttpPost]
        public async Task<JsonResult> TrayOut()
        {
            await ExecuteHardwareFunction(HardwareFunctionType.TrayOut, SignalType.TrayOut);

            return JsonSuccess(string.Empty);
        }

        [HttpPost]
        public async Task<JsonResult> TrayIn()
        {
            await ExecuteHardwareFunction(HardwareFunctionType.TrayIn, SignalType.TrayIn);

            return JsonSuccess(string.Empty);
        }

        [HttpPost]
        public async Task<JsonResult> TrayLightOn()
        {
            await ExecuteHardwareFunction(HardwareFunctionType.TrayLightOn, SignalType.TrayLightOn);

            return JsonSuccess(string.Empty);
        }

        [HttpPost]
        public async Task<JsonResult> TrayLightOff()
        {
            await ExecuteHardwareFunction(HardwareFunctionType.TrayLightOff, SignalType.TrayLightOff);

            return JsonSuccess(string.Empty);
        }
        
        [HttpPost]
        public async Task<JsonResult> SetSyringePump(ManualControlViewModel manualControlViewModel)
        {
            try
            {
                User currentUser = manualControlService.GetCurrentUser();

                HardwareFunction syringePumpHardwareFunction =
                    await unitOfWork.GetAll<HardwareFunction>()
                        .SingleAsync(x => x.FunctionType == manualControlViewModel.SelectedSyringePumpFunction);

                string apiUrl;
                if (manualControlViewModel.SelectedSyringePumpFunction == HardwareFunctionType.SyringePumpDraw)
                {
                    apiUrl = string.Format(syringePumpHardwareFunction.ApiUrl,
                        manualControlViewModel.SyringePumpStepMode ? 1 : 0, manualControlViewModel.SyringePumpFlow, manualControlViewModel.SyringePumpVolume);
                }
                else
                {
                    apiUrl = string.Format(syringePumpHardwareFunction.ApiUrl,
                        manualControlViewModel.SyringePumpStepMode ? 1 : 0, manualControlViewModel.SyringePumpFlow);
                }

                await signalHubContext.Clients.Group(currentUser.Id).addMessage(
                    new SignalMessage
                    {
                        Message = apiUrl,
                        SignalType = SignalType.SyringePump
                    });

                return JsonSuccess(string.Empty);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ExecGSValveFunction(ManualControlViewModel manualControlViewModel)
        {
            try
            {
                User currentUser = manualControlService.GetCurrentUser();

                await signalHubContext.Clients.Group(currentUser.Id).addMessage(
                    new SignalMessage
                    {
                        Message = manualControlViewModel.SelectedGSValveFunction,
                        SignalType = SignalType.GSValve
                    });

                return JsonSuccess(string.Empty);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
        }

        private async Task ExecuteHardwareFunction(HardwareFunctionType hardwareFunctionType, SignalType signalType)
        {
            HardwareFunctionItemViewModel hardwareFunctionItemViewModel = await manualControlService.GetHardwareFunction(hardwareFunctionType);

            User currentUser = manualControlService.GetCurrentUser();

            await signalHubContext.Clients.Group(currentUser.Id).addMessage(
                new SignalMessage
                {
                    Message = JsonConvert.SerializeObject(hardwareFunctionItemViewModel),
                    SignalType = signalType
                });
        }
    }
}