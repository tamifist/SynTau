using System.Linq;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Data.Contracts;
using Data.Contracts.Entities.CycleEditor;
using Shared.Enum;

namespace AutoGene.Presentation.Host.Controllers.SystemConfiguration
{
    public class ActivateChannelFunctionController : ODataController
    {
        private readonly IUnitOfWork unitOfWork;

        public ActivateChannelFunctionController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<HardwareFunction> Get()
        {
            var entities = unitOfWork.GetAll<HardwareFunction>().Where(x => x.FunctionType == HardwareFunctionType.ActivateChannel);
            return entities;
        }
    }
}