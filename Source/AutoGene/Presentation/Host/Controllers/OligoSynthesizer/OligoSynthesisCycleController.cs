using System.Linq;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Data.Contracts;
using Data.Contracts.Entities.CycleEditor;

namespace AutoGene.Presentation.Host.Controllers.OligoSynthesizer
{
    public class OligoSynthesisCycleController : ODataController
    {
        private readonly IUnitOfWork unitOfWork;

        public OligoSynthesisCycleController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<SynthesisCycle> Get()
        {
            var synthesisCycles = unitOfWork.GetAll<SynthesisCycle>();
            return synthesisCycles;
        }
    }
}