using System.Threading.Tasks;
using Data.Common.Contracts.Entities;
using Data.Ecommerce.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Common.Controllers;

namespace Presentation.Host.Areas.Ecommerce.Controllers
{
    [Area("Ecommerce")]
    [Authorize(Policy = "Admin")] //AdminRequirement
    //[Authorize]
    public class HomeController : BaseController
    {
        private readonly IEcommerceUnitOfWork dbContext;

        public HomeController(IEcommerceUnitOfWork dbContext)
        {
            this.dbContext = dbContext;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var test = await dbContext.GetAll<User>().ToListAsync();
            return View();
        }
    }
}
