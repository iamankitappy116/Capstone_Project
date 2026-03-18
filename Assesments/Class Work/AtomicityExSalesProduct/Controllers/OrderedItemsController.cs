using AtomicityExSalesProduct.Data;
using AtomicityExSalesProduct.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AtomicityExSalesProduct.Controllers
{
    public class OrderedItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderedItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrderedItems
        public async Task<IActionResult> Index()
        {
            var orderedItems = await _context.OrderedItems
                .Include(o => o.Product)
                .Include(o => o.AppUser)
                .ToListAsync();
            return View(orderedItems);
        }

        // GET: OrderedItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var orderedItem = await _context.OrderedItems
                .Include(o => o.Product)
                .Include(o => o.AppUser)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orderedItem == null)
                return NotFound();

            return View(orderedItem);
        }

        // GET: OrderedItems/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Name");
            return View();
        }

        // POST: OrderedItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,Quantity,AppUserId,OrderedAt")] OrderedItem orderedItem)
        {
            if (ModelState.IsValid)
            {
                orderedItem.OrderedAt = DateTime.Now;
                _context.Add(orderedItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", orderedItem.ProductId);
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Name", orderedItem.AppUserId);
            return View(orderedItem);
        }

        // GET: OrderedItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var orderedItem = await _context.OrderedItems.FindAsync(id);
            if (orderedItem == null)
                return NotFound();

            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", orderedItem.ProductId);
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Name", orderedItem.AppUserId);
            return View(orderedItem);
        }

        // POST: OrderedItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Quantity,AppUserId,OrderedAt")] OrderedItem orderedItem)
        {
            if (id != orderedItem.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderedItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.OrderedItems.Any(e => e.Id == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", orderedItem.ProductId);
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Name", orderedItem.AppUserId);
            return View(orderedItem);
        }

        // GET: OrderedItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var orderedItem = await _context.OrderedItems
                .Include(o => o.Product)
                .Include(o => o.AppUser)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orderedItem == null)
                return NotFound();

            return View(orderedItem);
        }

        // POST: OrderedItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderedItem = await _context.OrderedItems.FindAsync(id);
            if (orderedItem != null)
            {
                _context.OrderedItems.Remove(orderedItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
