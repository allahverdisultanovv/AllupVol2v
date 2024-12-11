using AllupVol2.Areas.Admin.ViewModels;
using AllupVol2.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupVol2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetProductVM> productVMs = await _context.Products.Include(p => p.Category).Select(p =>
            new GetProductVM()
            {
                Name = p.Name,
                Availability = p.Availability,
                CategoryName = p.Category.Name,
                Price = p.Price,
                Id = p.Id,
            }
            ).ToListAsync();
            return View(productVMs);
        }
        public async Task<IActionResult> Create()
        {

            return View();
        }
    }
}
