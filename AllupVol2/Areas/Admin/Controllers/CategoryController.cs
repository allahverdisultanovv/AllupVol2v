using AllupVol2.Areas.Admin.ViewModels;
using AllupVol2.DAL;
using AllupVol2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupVol2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<GetCategoryVM> categoryVMs = await _context.Categories.Select(x =>
            new GetCategoryVM()
            {
                Name = x.Name,
                Id = x.Id,
            }
                ).ToListAsync();
            return View(categoryVMs);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM categoryVM)
        {
            if (!ModelState.IsValid) return View();
            bool result = await _context.Categories.AnyAsync(c => c.Name == categoryVM.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(categoryVM.Name), "Category Already exists");
                return View();
            }
            Category category = new()
            {
                Name = categoryVM.Name,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            UpdateCategoryVM categoryVM = new()
            {
                Name = category.Name,
            };
            return View(categoryVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateCategoryVM categoryVM)
        {
            if (id < 1 || id is null) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            bool result = await _context.Categories.AnyAsync(c => c.Name == categoryVM.Name && c.Id != id);
            if (result)
            {
                ModelState.AddModelError(nameof(categoryVM.Name), "Category already exists");
                return View();
            };
            category.Name = categoryVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
