using System.Web.Http.Controllers;
using Data.Contracts.Entities;
using Data.Services;
using Microsoft.Azure.Mobile.Server;

namespace Mobile.Backend.Controllers.TableControllers
{
    public abstract class BaseTableController<T> : TableController<T> where T: Entity
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            CommonDbContext context = new CommonDbContext();
            DomainManager = new EntityDomainManager<T>(context, Request, enableSoftDelete: true);
        }
    }
}