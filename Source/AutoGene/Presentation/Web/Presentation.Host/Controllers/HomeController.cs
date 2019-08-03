using System.Linq;
using System.Threading.Tasks;
using Data.Common.Contracts;
using Data.Common.Contracts.Entities;
using Data.Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentation.Host.Controllers
{
    public class HomeController : Controller
    {
        private readonly EcommerceDbContext dbContext;

        public HomeController(EcommerceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var test = await dbContext.Set<User>().ToListAsync();
            return View();
        }
    }
}
