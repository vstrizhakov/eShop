using eShop.Database.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.Controllers
{
    [Authorize]
    public class ViberUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ViberUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ViberUsers
        public async Task<IActionResult> Index()
        {
            return _context.ViberUsers != null ?
                        View(await _context.ViberUsers.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.ViberUsers'  is null.");
        }

        // GET: ViberUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ViberUsers == null)
            {
                return NotFound();
            }

            var viberUser = await _context.ViberUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (viberUser == null)
            {
                return NotFound();
            }

            return View(viberUser);
        }

        // GET: ViberUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ViberUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ExternalId,Name,IsSubcribed")] ViberUser viberUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viberUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viberUser);
        }

        // GET: ViberUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ViberUsers == null)
            {
                return NotFound();
            }

            var viberUser = await _context.ViberUsers.FindAsync(id);
            if (viberUser == null)
            {
                return NotFound();
            }
            return View(viberUser);
        }

        // POST: ViberUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,ExternalId,Name,IsSubcribed")] ViberUser viberUser)
        {
            if (id != viberUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viberUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ViberUserExists(viberUser.Id))
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
            return View(viberUser);
        }

        // GET: ViberUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ViberUsers == null)
            {
                return NotFound();
            }

            var viberUser = await _context.ViberUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (viberUser == null)
            {
                return NotFound();
            }

            return View(viberUser);
        }

        // POST: ViberUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ViberUsers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ViberUsers'  is null.");
            }
            var viberUser = await _context.ViberUsers.FindAsync(id);
            if (viberUser != null)
            {
                _context.ViberUsers.Remove(viberUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ViberUserExists(string id)
        {
            return (_context.ViberUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
