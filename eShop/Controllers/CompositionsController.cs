using eShop.Database.Data;
using eShop.Extensions;
using eShop.Models.Compositions;
using eShop.Services;
using eShop.ViberBot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eShop.Controllers
{
    [Authorize]
    public class CompositionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public CompositionsController(ApplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        // GET: Compositions
        public async Task<IActionResult> Index()
        {
            var userId = User.GetSub();
            var compositions = await _context.Compositions
                .Where(e => e.OwnerId == userId)
                .ToListAsync();
            return View(compositions);
        }

        // GET: Compositions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.GetSub();
            var composition = await _context.Compositions
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (composition == null)
            {
                return NotFound();
            }

            return View(composition);
        }

        // GET: Compositions/Create
        public IActionResult Create([FromQuery] int capacity = 1)
        {
            ViewBag.Capacity = capacity;

            return View();
        }

        // POST: Compositions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateCompositionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.GetSub();
                var composition = new Composition
                {
                    OwnerId = userId,
                };

                var image = model.Image;
                using var imageStream = image.OpenReadStream();
                var imagePath = await _fileManager.SaveAsync(Path.Combine("Compositions", composition.Id, "Images"), Path.GetExtension(image.FileName), imageStream);

                composition.Images.Add(new CompositionImage
                {
                    Path = imagePath,
                });
                _context.Add(composition);

                var currency = await _context.Currencies.FirstOrDefaultAsync();
                if (currency == null)
                {
                    // handle & return
                }

                foreach (var compositionProduct in model.Products)
                {
                    var product = new Product
                    {
                        Name = compositionProduct.Name,
                        Url = compositionProduct.Url,
                        Prices = new List<ProductPrice>
                        {
                            new ProductPrice
                            {
                                Currency = currency,
                                Value = compositionProduct.Price,
                            },
                        },
                    };

                    composition.Products.Add(product);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Compositions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.GetSub();
            var composition = await _context.Compositions
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (composition == null)
            {
                return NotFound();
            }

            return View(composition);
        }

        // POST: Compositions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,CreatedAt")] Composition composition)
        {
            if (id != composition.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = User.GetSub();
                var compositionExists = await _context.Compositions
                    .Where(e => e.OwnerId == userId)
                    .AnyAsync(e => e.Id == id);
                if (!compositionExists)
                {
                    return NotFound();
                }

                try
                {
                    composition.OwnerId = userId;

                    _context.Update(composition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompositionExists(composition.Id))
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

            return View(composition);
        }

        // GET: Compositions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.GetSub();
            var composition = await _context.Compositions
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (composition == null)
            {
                return NotFound();
            }

            return View(composition);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] string id)
        {
            var userId = User.GetSub();
            var composition = await _context.Compositions
                .Include(e => e.Images)
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (composition != null)
            {
                _context.Compositions.Remove(composition);

                await _context.SaveChangesAsync();

                foreach (var image in composition.Images)
                {
                    await _fileManager.DeleteAsync(image.Path);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToTelegram(
            [FromRoute] string id,
            [FromServices] ITelegramBotClient botClient,
            [FromServices] IPublicUriBuilder publicUriBuilder)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.GetSub();
            var composition = await _context.Compositions
                .Include(e => e.Images)
                .Include(e => e.Products)
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (composition == null)
            {
                return NotFound();
            }

            var telegramChat = await _context.TelegramChats
                .FirstOrDefaultAsync(e => e.Type == ChatType.Channel);
            if (telegramChat != null)
            {
                var image = composition.Images.FirstOrDefault();
                if (image != null)
                {
                    var imageLink = publicUriBuilder.Path(image.Path);
                    var media = new InputMediaPhoto(new InputFileUrl(imageLink));
                    media.Caption = string.Join("\n\n", composition.Products.Select(e => $"{e.Name} - {e.Prices.LastOrDefault()}"));
                    await botClient.SendMediaGroupAsync(new ChatId(telegramChat.ExternalId), new List<IAlbumInputMedia>() { media });
                }
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToViber(
            [FromRoute] string id,
            [FromServices] IViberBotClient botClient,
            [FromServices] IPublicUriBuilder publicUriBuilder)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.GetSub();
            var composition = await _context.Compositions
                .Include(e => e.Images)
                .Include(e => e.Products)
                .Where(e => e.OwnerId == userId)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (composition == null)
            {
                return NotFound();
            }

            var viberUser = await _context.ViberUsers
                .Where(e => e.IsSubcribed)
                .FirstOrDefaultAsync();
            if (viberUser != null)
            {
                var image = composition.Images.FirstOrDefault();
                if (image != null)
                {
                    var imageLink = publicUriBuilder.Path(image.Path);
                    var text = string.Join("\n\n", composition.Products.Select(e => $"{e.Name} - {e.Prices.LastOrDefault()}"));
                    var sender = new ViberBot.User
                    {
                        Name = "Test",
                    };
                    await botClient.SendPictureMessageAsync(viberUser.ExternalId, sender, imageLink, text);
                }
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        private bool CompositionExists(string id)
        {
            return (_context.Compositions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
