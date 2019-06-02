using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using Data.Contracts.Entities.CycleEditor;

namespace Mobile.Backend.Controllers.TableControllers
{
    public class HardwareFunctionController : BaseTableController<HardwareFunction>
    {        
        public IQueryable<HardwareFunction> GetAllHardwareFunctions()
        {
            return Query();
        }
        
        public SingleResult<HardwareFunction> GetHardwareFunction(string id)
        {
            return Lookup(id);
        }
        
        public Task<HardwareFunction> PatchHardwareFunction(string id, Delta<HardwareFunction> patch)
        {
            return UpdateAsync(id, patch);
        }
        
        public async Task<IHttpActionResult> PostHardwareFunction(HardwareFunction item)
        {
            HardwareFunction current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }
        
        public Task DeleteHardwareFunction(string id)
        {
            return DeleteAsync(id);
        }
    }
}