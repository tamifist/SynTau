using System.Data.Entity;
using System.Threading.Tasks;
using Data.Contracts.Entities.Identity;
using Microsoft.Azure.Mobile.Server.Config;

namespace Mobile.Backend.Controllers.ApiControllers
{
    [MobileAppController]
    public class CustomAuthController : BaseApiController
    {
        public async Task<string> Get(string email, string password)
        {
            User user = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user != null)
            {
                return user.Id;
            }

            return null;
        }
    }
}
