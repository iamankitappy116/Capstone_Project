using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthFoodDeliveryApp.FoodDeliveryApp.Models;
using FoodDeliveryApp.Models;

namespace FoodDelivery.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private readonly FoodDeliveryDbCOntext _context;

        public SalesController(FoodDeliveryDbCOntext context)
        {
            _context = context;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(await _context.Sales.ToListAsync());
            }
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _context.Sales.Where(s => s.UserId == userId).ToListAsync());
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .FirstOrDefaultAsync(m => m.SaleId == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleId,TotalAmount,UserId,Date,Status")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sale);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SaleId,TotalAmount,UserId,Date,Status")] Sale sale)
        {
            if (id != sale.SaleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.SaleId))
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
            return View(sale);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .FirstOrDefaultAsync(m => m.SaleId == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Sales/Purchase
        [HttpPost]
        public async Task<IActionResult> Purchase()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Challenge();

            var cartItems = await _context.Carts
                .Include(c => c.Food)
                .Where(c => c.CustomerId == userId)
                .ToListAsync();

            if (!cartItems.Any()) return RedirectToAction("Index", "Foods");

            var sale = new Sale
            {
                UserId = userId,
                Date = DateTime.Now,
                Status = "Pending",
                TotalAmount = cartItems.Sum(c => c.TotalAmount)
            };

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync(); // Save to get SaleId

            foreach (var item in cartItems)
            {
                var productSold = new ProductsSold
                {
                    ProductId = item.FoodId,
                    SaleId = sale.SaleId,
                    Qty = item.Qty,
                    TotalProductAmount = item.TotalAmount,
                    Status = "Sold"
                };
                _context.ProductsSold.Add(productSold);
            }

            // Clear Cart
            _context.Carts.RemoveRange(cartItems);
            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
            return _context.Sales.Any(e => e.SaleId == id);
        }
    }
}
