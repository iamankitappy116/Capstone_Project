using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthFoodDeliveryApp.FoodDeliveryApp.Models;
using FoodDeliveryApp.Models;

namespace FoodDelivery.Controllers
{
    public class PartiesController : Controller
    {
        private readonly FoodDeliveryDbCOntext _context;

        public PartiesController(FoodDeliveryDbCOntext context)
        {
            _context = context;
        }

        // GET: Parties
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parties.ToListAsync());
        }

        // GET: Parties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var party = await _context.Parties.FirstOrDefaultAsync(m => m.Id == id);
            if (party == null) return NotFound();

            return View(party);
        }

        // GET: Parties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsExternal")] Party party)
        {
            if (ModelState.IsValid)
            {
                _context.Add(party);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(party);
        }

        // GET: Parties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var party = await _context.Parties.FindAsync(id);
            if (party == null) return NotFound();
            return View(party);
        }

        // POST: Parties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsExternal")] Party party)
        {
            if (id != party.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(party);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartyExists(party.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(party);
        }

        // GET: Parties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var party = await _context.Parties.FirstOrDefaultAsync(m => m.Id == id);
            if (party == null) return NotFound();

            return View(party);
        }

        // POST: Parties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var party = await _context.Parties.FindAsync(id);
            if (party != null)
            {
                _context.Parties.Remove(party);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 6. Signup Method
        public async Task<IActionResult> Signup()
        {
            ViewBag.PartyList = new SelectList(await _context.Parties.ToListAsync(), "Id", "Name");
            return View();
        }

        // 7. AllowSignUp Method
        [HttpGet]
        public async Task<JsonResult> AllowSignUp(int partyId)
        {
            var party = await _context.Parties.FindAsync(partyId);
            return Json(new { status = party.IsExternal ? "external" : "internal" });
        }

        private bool PartyExists(int id)
        {
            return _context.Parties.Any(e => e.Id == id);
        }
    }
}
