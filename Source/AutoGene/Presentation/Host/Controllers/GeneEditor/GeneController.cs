using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Business.Contracts.ViewModels.GeneEditor;
using Data.Contracts;
using Data.Contracts.Entities.GeneEditor;
using Shared.Framework.Security;

namespace AutoGene.Presentation.Host.Controllers.GeneEditor
{
    public class GeneController: ODataController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IIdentityStorage identityStorage;

        public GeneController(IUnitOfWork unitOfWork, IIdentityStorage identityStorage)
        {
            this.unitOfWork = unitOfWork;
            this.identityStorage = identityStorage;
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<Gene> Get()
        {
            var currentPrincipal = identityStorage.GetPrincipal();
            var entities = unitOfWork.GetAll<Gene>().Where(x => x.UserId == currentPrincipal.UserId);
            return entities;
        }
    }
}