using AllupVol2.Areas.Admin.ViewModels;
using AllupVol2.DAL;
using AllupVol2.Models;
using AllupVol2.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupVol2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BrandController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetBrandVM> brandVMs = await _context.Brands.Select(b =>
                new GetBrandVM()
                {
                    Name = b.Name,
                    Id = b.Id,
                    Image = b.Image,
                }
            ).ToListAsync();
            return View(brandVMs);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBrandVM brandVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result = await _context.Brands.AnyAsync(b => b.Name == brandVM.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(brandVM.Name), "Category already exists");
                return View();
            }
            if (!brandVM.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError(nameof(brandVM.Image), "file type is incorrect");
                return View();
            }
            if (!brandVM.Image.CheckFileSize(Utilities.Enums.FileSize.MB, 2))
            {
                ModelState.AddModelError(nameof(brandVM.Image), "file size is incorrect");
                return View();
            }

            Brand brand = new()
            {
                Name = brandVM.Name,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                Image = await brandVM.Image.CreateFileAsync(_env.WebRootPath, "assets", "images")
            };
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
            Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
            if (brand == null) return NotFound();
            UpdateBrandVM brandVM = new()
            {
                Name = brand.Name,
                Image = brand.Image
            };
            return View(brandVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateBrandVM brandVM)
        {
            if (id < 1 || id is null) return BadRequest();
            Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
            if (brand == null) return NotFound();
            bool result = await _context.Brands.AnyAsync(b => b.Id != id && b.Name == brandVM.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(brandVM.Name), "Brand already exists");
                return View(brandVM);
            }

            if (brandVM.Photo is not null)
            {
                if (!brandVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError(nameof(brandVM.Photo), "file type is incorrect");
                    return View();
                }
                if (!brandVM.Photo.CheckFileSize(Utilities.Enums.FileSize.MB, 2))
                {
                    ModelState.AddModelError(nameof(brandVM.Photo), "file size is incorrect");
                    return View();
                }
                string fileName = await brandVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images");
                brand.Image.DeleteFile(_env.WebRootPath, "assets", "images");
                brandVM.Image = fileName;
            }
            brand.Name = brandVM.Name;
            brand.Image = brandVM.Image;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
            if (brand == null) return NotFound();
            brand.Image.DeleteFile(_env.WebRootPath, "assets", "images");
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
