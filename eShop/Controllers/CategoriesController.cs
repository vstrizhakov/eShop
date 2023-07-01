using eShop.Database.Data;
using eShop.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eShop.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var userId = User.GetSub();
            var categories = await _context.Categories
                .Where(e => e.OwnerId == userId)
                .Include(c => c.ParentCategory)
                .ToListAsync();
            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.GetSub();
            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public async Task<IActionResult> Create()
        {
            var userId = User.GetSub();
            var categories = await _context.Categories
                .Where(e => e.OwnerId == userId)
                .ToListAsync();
            ViewData["ParentCategoryId"] = new SelectList(categories, "Id", nameof(Category.Name));
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            // TODO: check parent category for belonging to user

            var userId = User.GetSub();
            if (ModelState.IsValid)
            {
                category.OwnerId = userId;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var categories = await _context.Categories
                .Where(e => e.OwnerId == userId)
                .ToListAsync();
            ViewData["ParentCategoryId"] = new SelectList(categories, "Id", nameof(Category.Name), category.ParentCategoryId);

            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var userId = User.GetSub();
            var category = await _context.Categories
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories
                .Where(e => e.OwnerId == userId)
                .Where(e => e.Id != category.Id)
                .ToListAsync();
            ViewData["ParentCategoryId"] = new SelectList(categories, "Id", nameof(Category.Name), category.ParentCategoryId);

            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,ParentCategoryId,CreatedAt")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            var userId = User.GetSub();
            if (ModelState.IsValid)
            {
                var categoryExists = await _context.Categories
                    .Where(e => e.OwnerId == userId)
                    .AnyAsync();
                if (!categoryExists)
                {
                    return NotFound();
                }

                try
                {
                    category.OwnerId = userId;
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            var categories = await _context.Categories
                .Where(e => e.OwnerId == userId)
                .Where(e => e.Id != category.Id)
                .ToListAsync();
            ViewData["ParentCategoryId"] = new SelectList(categories, "Id", nameof(Category.Name), category.ParentCategoryId);

            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var userId = User.GetSub();
            var category = await _context.Categories
                .Include(e => e.ParentCategory)
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userId = User.GetSub();
            var category = await _context.Categories
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(string id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
