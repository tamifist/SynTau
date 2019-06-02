using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using Data.Contracts.Entities.Settings;

namespace Mobile.Backend.Controllers.TableControllers
{
    public class SynthesizerSettingController : BaseTableController<SynthesizerSetting>
    {
        public IQueryable<SynthesizerSetting> GetAllSynthesizerSettings()
        {
            return Query();
        }
        
        public SingleResult<SynthesizerSetting> GetSynthesizerSetting(string id)
        {
            return Lookup(id);
        }
        
        public Task<SynthesizerSetting> PatchSynthesizerSetting(string id, Delta<SynthesizerSetting> patch)
        {
            return UpdateAsync(id, patch);
        }
        
        public async Task<IHttpActionResult> PostSynthesizerSetting(SynthesizerSetting item)
        {
            SynthesizerSetting current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }
        
        public Task DeleteSynthesizerSetting(string id)
        {
            return DeleteAsync(id);
        }
    }
}