using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Data.Contracts.Entities.OligoSynthesizer;
using Data.Contracts.Entities.Settings;
using Data.Services;
using Microsoft.Azure.Mobile.Server;
using Mobile.Backend.ActionFilters;
using Shared.Enum;

namespace Mobile.Backend.Controllers.TableControllers
{
    public class OligoSynthesisProcessController : BaseTableController<OligoSynthesisProcess>
    {
        [QueryableExpand("OligoSynthesisActivities/ChannelApiFunction,OligoSynthesisActivities/SynthesisCycle/CycleSteps/HardwareFunction")]
        [EnableQuery(MaxExpansionDepth = 4, MaxTop = 1000)]
        public IQueryable<OligoSynthesisProcess> GetAllOligoSynthesisProcess()
        {
            //IQueryable<OligoSynthesisProcess> query = Query()
            //    .Include(x => x.OligoSynthesisActivities)
            //    .Include(x => x.OligoSynthesisActivities.Select(y => y.SynthesisCycle))
            //    .Include(x => x.OligoSynthesisActivities.Select(y => y.SynthesisCycle.CycleSteps))
            //    .Where(x => x.Status == SynthesisProcessStatus.InProgress);

            //return query.ToList().AsQueryable();
            IQueryable<OligoSynthesisProcess> query = Query().Where(x => x.Status == SynthesisProcessStatus.InProgress);
            return query;
        }
        
        public SingleResult<OligoSynthesisProcess> GetOligoSynthesisProcess(string id)
        {
            return Lookup(id);
        }
        
        public Task<OligoSynthesisProcess> PatchOligoSynthesisProcess(string id, Delta<OligoSynthesisProcess> patch)
        {
            return UpdateAsync(id, patch);
        }
        
        public async Task<IHttpActionResult> PostOligoSynthesisProcess(OligoSynthesisProcess item)
        {
            OligoSynthesisProcess current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        public Task DeleteOligoSynthesisProcess(string id)
        {
            return DeleteAsync(id);
        }
    }
}