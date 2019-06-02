using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoGene.Presentation.Host.Hubs;
using Business.Contracts.Services.OligoSynthesizer;
using Business.Contracts.ViewModels.OligoSynthesizer;
using Data.Contracts.Entities.Identity;
using Microsoft.AspNet.SignalR;
using Presentation.Common.Controllers;
using Presentation.Common.Security;
using Shared.DTO.Responses;
using Shared.Enum;
using Shared.Resources;

namespace AutoGene.Presentation.Host.Controllers.OligoSynthesizer
{
    [AuthorizeUser(UserRoles = "Guest,Admin")]
    public class OligoSynthesizerController : BaseController
    {
        private readonly IOligoSynthesizerService oligoSynthesizerService;
        private readonly IHubContext signalHubContext;

        public OligoSynthesizerController(IOligoSynthesizerService oligoSynthesizerService)
        {
            this.oligoSynthesizerService = oligoSynthesizerService;
            signalHubContext = GlobalHost.ConnectionManager.GetHubContext<SignalHub>();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            SynthesisProcessViewModel currentSynthesisProcessViewModel = await oligoSynthesizerService.GetCurrentSynthesisProcess();

            return View(currentSynthesisProcessViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> StartSynthesis(SynthesisProcessViewModel synthesisProcessViewModel)
        {
            try
            {
                await oligoSynthesizerService.StartSynthesis();

                User currentUser = oligoSynthesizerService.GetCurrentUser();

                await signalHubContext.Clients.Group(currentUser.Id).addMessage(
                    new SignalMessage
                    {
                        SignalType = SignalType.StartOligoSynthesisProcess
                    });
                return JsonSuccess(string.Empty);
            }
            catch (Exception ex)
            {
                return JsonError();
            }
        }

        [HttpPost]
        public async Task<JsonResult> StopSynthesis(SynthesisProcessViewModel synthesisProcessViewModel)
        {
            try
            {
                await oligoSynthesizerService.StopSynthesis();

                User currentUser = oligoSynthesizerService.GetCurrentUser();

                await signalHubContext.Clients.Group(currentUser.Id).addMessage(
                    new SignalMessage
                    {
                        SignalType = SignalType.SuspendOligoSynthesisProcess
                    });
                return JsonSuccess(string.Empty);
            }
            catch (Exception ex)
            {
                return JsonError();
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteSynthesisProcess(SynthesisProcessViewModel synthesisProcessViewModel)
        {
            try
            {
                await oligoSynthesizerService.DeleteSynthesisProcess(synthesisProcessViewModel.Id);

                User currentUser = oligoSynthesizerService.GetCurrentUser();

                await signalHubContext.Clients.Group(currentUser.Id).addMessage(
                    new SignalMessage
                    {
                        SignalType = SignalType.StopOligoSynthesisProcess
                    });
                return JsonSuccess(string.Empty);
            }
            catch (Exception ex)
            {
                return JsonError();
            }
        }
    }
}