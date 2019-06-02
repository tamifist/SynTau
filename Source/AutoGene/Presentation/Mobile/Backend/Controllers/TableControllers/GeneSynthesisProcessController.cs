using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using Data.Contracts.Entities.GeneSynthesizer;
using Data.Contracts.Entities.OligoSynthesizer;
using Mobile.Backend.ActionFilters;
using Shared.Enum;

namespace Mobile.Backend.Controllers.TableControllers
{
    public class GeneSynthesisProcessController : BaseTableController<GeneSynthesisProcess>
    {
        [QueryableExpand("GeneSynthesisActivities/ChannelApiFunction,GeneSynthesisActivities/SynthesisCycle/CycleSteps/HardwareFunction,Gene")]
        [EnableQuery(MaxExpansionDepth = 4, MaxTop = 1000)]
        public IQueryable<GeneSynthesisProcess> GetAllGeneSynthesisProcess()
        {
            IQueryable<GeneSynthesisProcess> query = Query().Where(x => x.Status == SynthesisProcessStatus.InProgress);
            return query;
        }
        
        public SingleResult<GeneSynthesisProcess> GetGeneSynthesisProcess(string id)
        {
            return Lookup(id);
        }
        
        public Task<GeneSynthesisProcess> PatchGeneSynthesisProcess(string id, Delta<GeneSynthesisProcess> patch)
        {
            return UpdateAsync(id, patch);
        }
        
        public async Task<IHttpActionResult> PostGeneSynthesisProcess(GeneSynthesisProcess item)
        {
            GeneSynthesisProcess current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        public Task DeleteGeneSynthesisProcess(string id)
        {
            return DeleteAsync(id);
        }
    }
}