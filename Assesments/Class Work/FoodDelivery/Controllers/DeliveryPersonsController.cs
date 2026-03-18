using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.Data;
using FoodDelivery.Models;

namespace FoodDelivery.Controllers
{
    public class DeliveryPersonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeliveryPersonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DeliveryPersons
        public async Task<IActionResult> Index()
        {
            return View(await _context.DeliveryPersons.ToListAsync());
        }

        // GET: DeliveryPersons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryPerson = await _context.DeliveryPersons
                .FirstOrDefaultAsync(m => m.DeliveryPersonId == id);
            if (deliveryPerson == null)
            {
                return NotFound();
            }

            return View(deliveryPerson);
        }

        // GET: DeliveryPersons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeliveryPersons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeliveryPersonId,Name,Phone")] DeliveryPerson deliveryPerson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deliveryPerson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deliveryPerson);
        }

        // GET: DeliveryPersons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryPerson = await _context.DeliveryPersons.FindAsync(id);
            if (deliveryPerson == null)
            {
                return NotFound();
            }
            return View(deliveryPerson);
        }

        // POST: DeliveryPersons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeliveryPersonId,Name,Phone")] DeliveryPerson deliveryPerson)
        {
            if (id != deliveryPerson.DeliveryPersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryPerson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryPersonExists(deliveryPerson.DeliveryPersonId))
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
            return View(deliveryPerson);
        }

        // GET: DeliveryPersons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryPerson = await _context.DeliveryPersons
                .FirstOrDefaultAsync(m => m.DeliveryPersonId == id);
            if (deliveryPerson == null)
            {
                return NotFound();
            }

            return View(deliveryPerson);
        }

        // POST: DeliveryPersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deliveryPerson = await _context.DeliveryPersons.FindAsync(id);
            if (deliveryPerson != null)
            {
                _context.DeliveryPersons.Remove(deliveryPerson);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryPersonExists(int id)
        {
            return _context.DeliveryPersons.Any(e => e.DeliveryPersonId == id);
        }
    }
}
