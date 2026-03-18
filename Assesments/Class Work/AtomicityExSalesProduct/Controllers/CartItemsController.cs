using AtomicityExSalesProduct.Data;
using AtomicityExSalesProduct.Models;
using AtomicityExSalesProduct.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AtomicityExSalesProduct.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SalesService _salesService;

        public CartItemsController(ApplicationDbContext context, SalesService salesService)
        {
            _context = context;
            _salesService = salesService;
        }

        // GET: CartItems
        public async Task<IActionResult> Index()
        {
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Include(c => c.AppUser)
                .ToListAsync();
            return View(cartItems);
        }

        // GET: CartItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var cartItem = await _context.CartItems
                .Include(c => c.Product)
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cartItem == null)
                return NotFound();

            return View(cartItem);
        }

        // GET: CartItems/Create
        public IActionResult Create()
        {
            ViewData["ProductId"]  = new SelectList(_context.Products, "Id", "Name");
            ViewData["AppUserId"]  = new SelectList(_context.AppUsers, "Id", "Name");
            return View();
        }

        // POST: CartItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,Quantity,AppUserId")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cartItem.ProductId);
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Name", cartItem.AppUserId);
            return View(cartItem);
        }

        // GET: CartItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
                return NotFound();

            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cartItem.ProductId);
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Name", cartItem.AppUserId);
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Quantity,AppUserId")] CartItem cartItem)
        {
            if (id != cartItem.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.CartItems.Any(e => e.Id == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cartItem.ProductId);
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Name", cartItem.AppUserId);
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var cartItem = await _context.CartItems
                .Include(c => c.Product)
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cartItem == null)
                return NotFound();

            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // ---------------------------------------------------------------
        // Checkout — THE ATOMIC OPERATION
        // This calls SalesService.AtomicCheckout which:
        //   1. Deletes this CartItem
        //   2. Inserts an OrderedItem
        //   Both in ONE transaction — atomically.
        // GET: CartItems/Checkout/5
        // ---------------------------------------------------------------
        public async Task<IActionResult> Checkout(int? id)
        {
            if (id == null)
                return NotFound();

            var cartItem = await _context.CartItems
                .Include(c => c.Product)
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cartItem == null)
                return NotFound();

            return View(cartItem); // Confirmation page
        }

        // POST: CartItems/Checkout/5
        [HttpPost, ActionName("Checkout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutConfirmed(int id)
        {
            var success = await _salesService.AtomicCheckout(id);

            if (success)
            {
                TempData["Message"] = "Order placed successfully! Cart item removed and order recorded atomically.";
                return RedirectToAction("Index", "OrderedItems");
            }
            else
            {
                TempData["Error"] = "Checkout failed. Transaction was rolled back — cart is unchanged.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
