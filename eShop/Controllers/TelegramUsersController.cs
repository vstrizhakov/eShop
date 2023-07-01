using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using eShop.Database.Data;

namespace eShop.Controllers
{
    [Authorize]
    public class TelegramUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TelegramUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TelegramUsers
        public async Task<IActionResult> Index()
        {
              return _context.TelegramUsers != null ? 
                          View(await _context.TelegramUsers.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.TelegramUsers'  is null.");
        }

        // GET: TelegramUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.TelegramUsers == null)
            {
                return NotFound();
            }

            var telegramUser = await _context.TelegramUsers
                .Include(e => e.Chats)
                    .ThenInclude(e => e.Chat)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (telegramUser == null)
            {
                return NotFound();
            }

            return View(telegramUser);
        }

        // GET: TelegramUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TelegramUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Username,ExternalId")] TelegramUser telegramUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(telegramUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(telegramUser);
        }

        // GET: TelegramUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.TelegramUsers == null)
            {
                return NotFound();
            }

            var telegramUser = await _context.TelegramUsers.FindAsync(id);
            if (telegramUser == null)
            {
                return NotFound();
            }
            return View(telegramUser);
        }

        // POST: TelegramUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,Username,ExternalId")] TelegramUser telegramUser)
        {
            if (id != telegramUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(telegramUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TelegramUserExists(telegramUser.Id))
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
            return View(telegramUser);
        }

        // GET: TelegramUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.TelegramUsers == null)
            {
                return NotFound();
            }

            var telegramUser = await _context.TelegramUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (telegramUser == null)
            {
                return NotFound();
            }

            return View(telegramUser);
        }

        // POST: TelegramUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.TelegramUsers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TelegramUsers'  is null.");
            }
            var telegramUser = await _context.TelegramUsers.FindAsync(id);
            if (telegramUser != null)
            {
                _context.TelegramUsers.Remove(telegramUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TelegramUserExists(string id)
        {
          return (_context.TelegramUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
