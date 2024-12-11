using AllupVol2.Areas.Admin.ViewModels;
using AllupVol2.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupVol2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM vm = new HomeVM()
            {
                categoryVms = await _context.Categories.Select(c => new GetCategoryVM() { Name = c.Name, Id = c.Id }).ToListAsync(),
                slideVMs = await _context.Slides.Select(s => new GetSlideVM() { Maintitle = s.Maintitle, Order = s.Order, Image = s.Image, Id = s.Id }).ToListAsync(),
                brandVMs = await _context.Brands.Select(b => new GetBrandVM() { Name = b.Name, Image = b.Image, Id = b.Id }).ToListAsync(),
                tagVMs = await _context.Tags.Select(t => new GetTagVM() { Name = t.Name, Id = t.Id }).ToListAsync(),
                productVMs = await _context.Products.Include(p => p.Category).Select(p => new GetProductVM() { Price = p.Price, Availability = p.Availability, Name = p.Name, Id = p.Id, CategoryName = p.Category.Name }).ToListAsync()
            };
            return View(vm);
        }
    }
}
