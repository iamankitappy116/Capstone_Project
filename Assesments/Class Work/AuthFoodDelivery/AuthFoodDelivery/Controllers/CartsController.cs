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
    public class CartsController : Controller
    {
        private readonly FoodDeliveryDbCOntext _context;

        public CartsController(FoodDeliveryDbCOntext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var foodDeliveryDbCOntext = _context.Carts
                .Include(c => c.Food)
                .Where(c => c.CustomerId == userId);
            
            return View(await foodDeliveryDbCOntext.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Food)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["FoodId"] = new SelectList(_context.Foods, "Id", "Description");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FoodId,Qty,Price,TotalAmount,CustomerId")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FoodId"] = new SelectList(_context.Foods, "Id", "Description", cart.FoodId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["FoodId"] = new SelectList(_context.Foods, "Id", "Description", cart.FoodId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FoodId,Qty,Price,TotalAmount,CustomerId")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
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
            ViewData["FoodId"] = new SelectList(_context.Foods, "Id", "Description", cart.FoodId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Food)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Carts/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int foodId, int qty = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Challenge();

            var food = await _context.Foods.FindAsync(foodId);
            if (food == null) return NotFound();

            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.FoodId == foodId && c.CustomerId == userId);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    FoodId = foodId,
                    Qty = qty,
                    Price = food.Price,
                    TotalAmount = food.Price * qty,
                    CustomerId = userId
                };
                _context.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Qty += qty;
                cartItem.TotalAmount = cartItem.Qty * cartItem.Price;
                _context.Carts.Update(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int id, int change)
        {
            var cartItem = await _context.Carts.FindAsync(id);
            if (cartItem == null) return NotFound();

            cartItem.Qty += change;
            if (cartItem.Qty <= 0)
            {
                _context.Carts.Remove(cartItem);
            }
            else
            {
                cartItem.TotalAmount = cartItem.Qty * cartItem.Price;
                _context.Update(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
