using System.Linq;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Data.Contracts;
using Data.Contracts.Entities.CycleEditor;
using Shared.Enum;

namespace AutoGene.Presentation.Host.Controllers.ManualControl
{
    public class ValveFunctionController : ODataController
    {
        private readonly IUnitOfWork unitOfWork;

        public ValveFunctionController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<HardwareFunction> Get()
        {
            var valveHardwareFunctions = unitOfWork.GetAll<HardwareFunction>().Where(x => x.FunctionType == HardwareFunctionType.Valve && x.IsActivated);
            return valveHardwareFunctions;
        }
    }
}