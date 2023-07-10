using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using eShop.Database.Data;

namespace eShop.MVC.Controllers
{
    [Authorize]
    public class TelegramChatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TelegramChatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TelegramChats
        public async Task<IActionResult> Index()
        {
            return _context.TelegramChats != null ?
                        View(await _context.TelegramChats.Where(e => e.SupergroupId == null).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.TelegramChats'  is null.");
        }

        // GET: TelegramChats/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.TelegramChats == null)
            {
                return NotFound();
            }

            var telegramChat = await _context.TelegramChats
                .Include(e => e.Members)
                    .ThenInclude(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (telegramChat == null)
            {
                return NotFound();
            }

            return View(telegramChat);
        }

        // GET: TelegramChats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TelegramChats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ExternalId,Type")] TelegramChat telegramChat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(telegramChat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(telegramChat);
        }

        // GET: TelegramChats/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.TelegramChats == null)
            {
                return NotFound();
            }

            var telegramChat = await _context.TelegramChats.FindAsync(id);
            if (telegramChat == null)
            {
                return NotFound();
            }
            return View(telegramChat);
        }

        // POST: TelegramChats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,ExternalId,Type")] TelegramChat telegramChat)
        {
            if (id != telegramChat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(telegramChat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TelegramChatExists(telegramChat.Id))
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
            return View(telegramChat);
        }

        // GET: TelegramChats/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.TelegramChats == null)
            {
                return NotFound();
            }

            var telegramChat = await _context.TelegramChats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (telegramChat == null)
            {
                return NotFound();
            }

            return View(telegramChat);
        }

        // POST: TelegramChats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.TelegramChats == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TelegramChats'  is null.");
            }
            var telegramChat = await _context.TelegramChats.FindAsync(id);
            if (telegramChat != null)
            {
                _context.TelegramChats.Remove(telegramChat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TelegramChatExists(string id)
        {
            return (_context.TelegramChats?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
