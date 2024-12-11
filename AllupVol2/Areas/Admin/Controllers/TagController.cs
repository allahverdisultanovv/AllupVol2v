using AllupVol2.Areas.Admin.ViewModels;
using AllupVol2.DAL;
using AllupVol2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllupVol2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<GetTagVM> tagVms = await _context.Tags.Select(t =>
                new GetTagVM()
                {
                    Name = t.Name,
                    Id = t.Id,
                }
                )
                .ToListAsync();
            return View(tagVms);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTagVM createTag)
        {
            if (!ModelState.IsValid) return View();
            bool result = await _context.Tags.AnyAsync(t => t.Name == createTag.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(createTag.Name), "Category already exists");
                return View();
            }
            Tag tag = new Tag()
            {
                Name = createTag.Name,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
            };
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
            Tag tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (tag == null) return NotFound();
            UpdateTagVM tagVM = new()
            {
                Name = tag.Name,
            };
            return View(tagVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateTagVM tagVM)
        {
            if (id < 1 || id is null) return BadRequest();
            if (!ModelState.IsValid) return View(tagVM);
            Tag tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (tag == null) return NotFound();
            bool result = await _context.Tags.AnyAsync(t => t.Id != id && t.Name == tagVM.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(tagVM.Name), "This tag already exists");
                return View(tagVM);
            }
            tag.Name = tagVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
            Tag tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (tag == null) return NotFound();
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
