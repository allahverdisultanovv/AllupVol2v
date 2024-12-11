
using AllupVol2.Areas.Admin.ViewModels;
using AllupVol2.DAL;
using AllupVol2.Models;
using AllupVol2.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupVol2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SlideController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetSlideVM> slideVMs = await _context.Slides.Select(s =>
                new GetSlideVM()
                {
                    Id = s.Id,
                    Image = s.Image,
                    Maintitle = s.Maintitle,
                    Order = s.Order
                }
            ).ToListAsync();
            return View(slideVMs);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        {
            if (!ModelState.IsValid) return View();
            bool result = await _context.Slides.AnyAsync(s => s.Maintitle == slideVM.Maintitle);
            if (result)
            {
                ModelState.AddModelError(nameof(slideVM.Maintitle), "Slide are Exists");
            }
            if (!slideVM.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError(nameof(slideVM.Image), "file type is incorrect");
                return View();
            }
            if (!slideVM.Image.CheckFileSize(Utilities.Enums.FileSize.MB, 2))
            {
                ModelState.AddModelError(nameof(slideVM.Image), "file size is incorrect");
                return View();
            }

            Slide slide = new Slide()
            {
                Maintitle = slideVM.Maintitle,
                Order = slideVM.Order,
                Description = slideVM.Description,
                TitleDesc = slideVM.TitleDesc,
                Subtitle = slideVM.Subtitle,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                Image = await slideVM.Image.CreateFileAsync(_env.WebRootPath, "assets", "images")
            };
            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Slide slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide == null) return NotFound();
            UpdateSlideVM slideVM = new UpdateSlideVM()
            {
                Description = slide.Description,
                TitleDesc = slide.TitleDesc,
                Subtitle = slide.Subtitle,
                Order = slide.Order,
                Image = slide.Image,
                Maintitle = slide.Maintitle
            };
            return View(slideVM);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSlideVM slideVM)
        {
            if (id == null || id < 1) return BadRequest();
            if (!ModelState.IsValid) return View(slideVM);
            Slide slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide == null) return NotFound();
            if (slideVM.Photo is not null)
            {

                if (!slideVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError(nameof(slideVM.Photo), "Type is incorrect");
                    return View(slideVM);
                }
                if (!slideVM.Photo.CheckFileSize(Utilities.Enums.FileSize.MB, 2))
                {
                    ModelState.AddModelError(nameof(slideVM.Photo), "Size must be less than 2MB");
                    return View(slideVM);
                }
                string fileName = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images");
                slide.Image.DeleteFile(_env.WebRootPath, "assets", "images");
                slide.Image = fileName;

            }
            slide.Order = slideVM.Order;
            slide.Subtitle = slideVM.Subtitle;
            slide.Description = slideVM.Description;
            slide.TitleDesc = slideVM.TitleDesc;
            slide.Maintitle = slideVM.Maintitle;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
            Slide slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide == null) return NotFound();
            slide.Image.DeleteFile(_env.WebRootPath, "assets", "images");
            _context.Slides.Remove(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
