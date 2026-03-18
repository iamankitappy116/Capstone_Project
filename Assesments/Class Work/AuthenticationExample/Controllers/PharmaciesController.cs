using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using Models;
using Microsoft.AspNetCore.Authorization; // REQUIRED

namespace AuthenticationExample.Controllers
{
    [Authorize(Roles = "Badmin")]   // Only Admin can access
    public class PharmaciesController : Controller
    {
        private readonly PharmacyContext _context;

        public PharmaciesController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: Pharmacies
        public async Task<IActionResult> Index()
        {
            return View(await _context.pharmacies.ToListAsync());
        }

        // GET: Pharmacies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var pharmacy = await _context.pharmacies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pharmacy == null)
                return NotFound();

            return View(pharmacy);
        }

        // GET: Pharmacies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pharmacies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pharmacy pharmacy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pharmacy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pharmacy);
        }

        // GET: Pharmacies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var pharmacy = await _context.pharmacies.FindAsync(id);

            if (pharmacy == null)
                return NotFound();

            return View(pharmacy);
        }

        // POST: Pharmacies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pharmacy pharmacy)
        {
            if (id != pharmacy.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(pharmacy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(pharmacy);
        }

        // GET: Pharmacies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var pharmacy = await _context.pharmacies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pharmacy == null)
                return NotFound();

            return View(pharmacy);
        }

        // POST: Pharmacies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pharmacy = await _context.pharmacies.FindAsync(id);

            if (pharmacy != null)
                _context.pharmacies.Remove(pharmacy);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PharmacyExists(int id)
        {
            return _context.pharmacies.Any(e => e.Id == id);
        }
    }
}
