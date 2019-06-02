using System.Linq;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Data.Contracts;
using Data.Contracts.Entities.CycleEditor;
using Shared.Enum;

namespace AutoGene.Presentation.Host.Controllers.CycleEditor
{
    public class CycleStepFunctionController : ODataController
    {
        private readonly IUnitOfWork unitOfWork;

        public CycleStepFunctionController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<HardwareFunction> Get()
        {
            IQueryable<HardwareFunction> entities = unitOfWork.GetAll<HardwareFunction>()
                .Where(x => x.FunctionType == HardwareFunctionType.CycleStep ||
                            x.FunctionType == HardwareFunctionType.CloseAllValves ||
                            x.FunctionType == HardwareFunctionType.BAndTetToCol)
                .OrderBy(x => x.Number);
            return entities;
        }
    }
}