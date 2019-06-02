using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Business.Contracts.ViewModels.Common;
using Business.Contracts.ViewModels.CycleEditor;
using Data.Contracts;
using Data.Contracts.Entities.CycleEditor;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Presentation.Common.Security;
using Shared.Enum;

namespace AutoGene.Presentation.Host.Controllers.ManualControl
{
    [AuthorizeUser(UserRoles = "Guest,Admin")]
    public class ValveMultiSelectController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ValveMultiSelectController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult> GetValves([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<HardwareFunctionItemViewModel> allValves = await GetAllValves();
            return Json(allValves.ToDataSourceResult(request));
        }

        private async Task<IEnumerable<HardwareFunctionItemViewModel>> GetAllValves()
        {
            IEnumerable<HardwareFunction> allValves =
                await unitOfWork.GetAll<HardwareFunction>()
                    .Where(x => x.FunctionType == HardwareFunctionType.Valve)
                    .OrderBy(x => x.Number)
                    .ToListAsync();

            return allValves.Select(x => new HardwareFunctionItemViewModel
            {
                Id = x.Id,
                Number = x.Number,
                Name = x.Name,
                ApiUrl = x.ApiUrl,
                FunctionType = x.FunctionType
            });
        }
    }
}