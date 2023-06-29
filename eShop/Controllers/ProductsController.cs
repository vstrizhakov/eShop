using eShop.Database.Data;
using eShop.Extensions;
using eShop.Models;
using eShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eShop.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public ProductsController(ApplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var userId = User.GetSub();
            var products = await _context.Products
                .Where(e => e.OwnerId == userId)
                .Include(p => p.Category)
                .ToListAsync();
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.GetSub();
            var product = await _context.Products
                .Where(e => e.OwnerId == userId)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var userId = User.GetSub();
            var categories = await _context.Categories
                .Where(e => e.OwnerId == userId)
                .ToListAsync();
            var currencies = await _context.Currencies
                .ToListAsync();

            ViewData["CategoryId"] = new SelectList(categories, nameof(Category.Id), nameof(Category.Name));
            ViewData["CurrencyId"] = new SelectList(currencies, nameof(Currency.Id), nameof(Currency.Name));

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CategoryId,CreatedAt")] Product product)
        {
            var userId = User.GetSub();
            if (ModelState.IsValid)
            {
                product.OwnerId = userId;

                _context.Add(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var categories = await _context.Categories
                .Where(e => e.OwnerId == userId)
                .ToListAsync();
            var currencies = await _context.Currencies
                .ToListAsync();

            ViewData["CategoryId"] = new SelectList(categories, nameof(Category.Id), nameof(Category.Name), product.CategoryId);
            ViewData["CurrencyId"] = new SelectList(currencies, nameof(Currency.Id), nameof(Currency.Name));

            return View(product);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.GetSub();
            var product = await _context.Products
                .Include(e => e.Prices)
                    .ThenInclude(e => e.Currency)
                .Include(e => e.Images)
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync(e => e.Id == id, HttpContext.RequestAborted);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories
                .Where(e => e.OwnerId == userId)
                .ToListAsync();
            var currencies = await _context.Currencies
                .ToListAsync();

            ViewData["CategoryId"] = new SelectList(categories, nameof(Category.Id), nameof(Category.Name), product.CategoryId);
            ViewData["CurrencyId"] = new SelectList(currencies, nameof(Currency.Id), nameof(Currency.Name));

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,CategoryId,CreatedAt")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            var userId = User.GetSub();
            if (ModelState.IsValid)
            {
                var productExists = await _context.Products
                    .Where(e => e.OwnerId == userId)
                    .AnyAsync(e => e.Id == id);
                if (!productExists)
                {
                    return NotFound();
                }

                try
                {
                    product.OwnerId = userId;

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
                .ToListAsync();
            var currencies = await _context.Currencies
                .ToListAsync();

            ViewData["CategoryId"] = new SelectList(categories, nameof(Category.Id), nameof(Category.Name), product.CategoryId);
            ViewData["CurrencyId"] = new SelectList(currencies, nameof(Currency.Id), nameof(Currency.Name));

            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.GetSub();
            var product = await _context.Products
                .Include(p => p.Category)
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userId = User.GetSub();
            var product = await _context.Products
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPrice([FromRoute] string id, [FromForm] ProductPrice price)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.GetSub();
            var product = await _context.Products
                .Include(e => e.Prices)
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                product.Prices.Add(price);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImages([FromRoute] string id, [FromForm] AddImagesViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.GetSub();
            var product = await _context.Products
                .Include(e => e.Images)
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync(e => e.Id == id, HttpContext.RequestAborted);
            if (product == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                foreach (var image in model.Images)
                {
                    using var imageStream = image.OpenReadStream();
                    var imagePath = await _fileManager.SaveAsync(Path.Combine("Products", product.Id, "Images"), Path.GetExtension(image.FileName), imageStream);
                    var productImage = new ProductImage
                    {
                        Path = imagePath,
                    };

                    product.Images.Add(productImage);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Edit), new { id });
        }

        private bool ProductExists(string id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
