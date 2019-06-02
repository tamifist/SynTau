using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using Data.Contracts.Entities.Diagnostic;

namespace Mobile.Backend.Controllers.TableControllers
{
    public class LogController: BaseTableController<Log>
    {
        public IQueryable<Log> GetAllLogs()
        {
            return Query().OrderByDescending(x => x.CreatedAt).Take(100);
        }

        public SingleResult<Log> GetLog(string id)
        {
            return Lookup(id);
        }

        public Task<Log> PatchLog(string id, Delta<Log> patch)
        {
            return UpdateAsync(id, patch);
        }

        public async Task<IHttpActionResult> PostLog(Log item)
        {
            Log current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        public Task DeleteLog(string id)
        {
            return DeleteAsync(id);
        }
    }
}