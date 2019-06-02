using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoGene.Presentation.Host.Hubs;
using Business.Contracts.Services.GeneSynthesizer;
using Business.Contracts.ViewModels.GeneSynthesizer;
using Data.Contracts.Entities.Identity;
using Microsoft.AspNet.SignalR;
using Presentation.Common.Controllers;
using Presentation.Common.Security;
using Shared.DTO.Responses;
using Shared.Enum;

namespace AutoGene.Presentation.Host.Controllers.GeneSynthesizer
{
    [AuthorizeUser(UserRoles = "Guest,Admin")]
    public class GeneSynthesizerController : BaseController
    {
        private readonly IGeneSynthesizerService geneSynthesizerService;
        private readonly IHubContext signalHubContext;

        public GeneSynthesizerController(IGeneSynthesizerService geneSynthesizerService)
        {
            this.geneSynthesizerService = geneSynthesizerService;
            signalHubContext = GlobalHost.ConnectionManager.GetHubContext<SignalHub>();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            GeneSynthesisProcessViewModel currentSynthesisProcessViewModel = await geneSynthesizerService.GetCurrentSynthesisProcess();

            return View(currentSynthesisProcessViewModel);
        }
        
        [HttpGet]
        public async Task<JsonResult> CreateGeneSynthesisProcess(string geneId)
        {
            try
            {
                await geneSynthesizerService.CreateGeneSynthesisProcess(geneId);

                GeneSynthesisProcessViewModel currentSynthesisProcessViewModel = await geneSynthesizerService.GetCurrentSynthesisProcess();

                return Json(currentSynthesisProcessViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonError(ex.Message);
            }
            
        }

        [HttpPost]
        public async Task<JsonResult> UpdateGeneSynthesisProcess(GeneSynthesisProcessViewModel synthesisProcessViewModel)
        {
            try
            {
                await geneSynthesizerService.UpdateSynthesisProcess(synthesisProcessViewModel);
                return JsonSuccess(string.Empty);
            }
            catch (Exception ex)
            {
                return JsonError();
            }
        }


        [HttpPost]
        public async Task<JsonResult> StartSynthesis(GeneSynthesisProcessViewModel synthesisProcessViewModel)
        {
            try
            {
                await geneSynthesizerService.StartSynthesis();

                User currentUser = geneSynthesizerService.GetCurrentUser();

                await signalHubContext.Clients.Group(currentUser.Id).addMessage(
                    new SignalMessage
                    {
                        SignalType = SignalType.StartGeneSynthesisProcess
                    });
                return JsonSuccess(string.Empty);
            }
            catch (Exception ex)
            {
                return JsonError();
            }
        }

        [HttpPost]
        public async Task<JsonResult> StopSynthesis(GeneSynthesisProcessViewModel synthesisProcessViewModel)
        {
            try
            {
                await geneSynthesizerService.StopSynthesis();

                User currentUser = geneSynthesizerService.GetCurrentUser();

                await signalHubContext.Clients.Group(currentUser.Id).addMessage(
                    new SignalMessage
                    {
                        SignalType = SignalType.SuspendGeneSynthesisProcess
                    });
                return JsonSuccess(string.Empty);
            }
            catch (Exception ex)
            {
                return JsonError();
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteSynthesisProcess(GeneSynthesisProcessViewModel synthesisProcessViewModel)
        {
            try
            {
                await geneSynthesizerService.DeleteSynthesisProcess(synthesisProcessViewModel.Id);

                User currentUser = geneSynthesizerService.GetCurrentUser();

                await signalHubContext.Clients.Group(currentUser.Id).addMessage(
                    new SignalMessage
                    {
                        SignalType = SignalType.StopGeneSynthesisProcess
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