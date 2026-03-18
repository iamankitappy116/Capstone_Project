using AtomicityExSalesProduct.Data;
using AtomicityExSalesProduct.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AtomicityExSalesProduct.Controllers
{
    public class AppUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AppUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppUsers.ToListAsync());
        }

        // GET: AppUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var appUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == id);
            if (appUser == null)
                return NotFound();

            return View(appUser);
        }

        // GET: AppUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appUser);
        }

        // GET: AppUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var appUser = await _context.AppUsers.FindAsync(id);
            if (appUser == null)
                return NotFound();

            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email")] AppUser appUser)
        {
            if (id != appUser.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.AppUsers.Any(e => e.Id == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appUser);
        }

        // GET: AppUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var appUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == id);
            if (appUser == null)
                return NotFound();

            return View(appUser);
        }

        // POST: AppUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appUser = await _context.AppUsers.FindAsync(id);
            if (appUser != null)
            {
                _context.AppUsers.Remove(appUser);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
