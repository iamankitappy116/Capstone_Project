using Microsoft.AspNetCore.Mvc;
using Crime.Models;
using Crime.DAO;
using Crime.Exceptions;

namespace Crime.Controllers
{
    public class VictimController : Controller
    {
        private readonly ICrimeService _crimeService;

        public VictimController(ICrimeService crimeService)
        {
            _crimeService = crimeService;
        }

        // GET: Victim
        public IActionResult Index()
        {
            var victims = _crimeService.GetAllVictims();
            return View(victims);
        }

        // GET: Victim/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var victim = _crimeService.GetVictimById(id.Value);
                return View(victim);
            }
            catch (VictimNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Victim/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Victim/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Victim victim)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _crimeService.CreateVictim(victim);
                    TempData["SuccessMessage"] = "Victim created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidDateException ex)
                {
                    ModelState.AddModelError("DateOfBirth", ex.Message);
                }
                catch (InvalidPhoneNumberException ex)
                {
                    ModelState.AddModelError("PhoneNumber", ex.Message);
                }
            }

            return View(victim);
        }

        // GET: Victim/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var victim = _crimeService.GetVictimById(id.Value);
                return View(victim);
            }
            catch (VictimNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Victim/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Victim victim)
        {
            if (id != victim.VictimId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _crimeService.UpdateVictim(victim);
                    TempData["SuccessMessage"] = "Victim updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (VictimNotFoundException ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidDateException ex)
                {
                    ModelState.AddModelError("DateOfBirth", ex.Message);
                }
                catch (InvalidPhoneNumberException ex)
                {
                    ModelState.AddModelError("PhoneNumber", ex.Message);
                }
            }

            return View(victim);
        }

        // GET: Victim/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var victim = _crimeService.GetVictimById(id.Value);
                return View(victim);
            }
            catch (VictimNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Victim/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _crimeService.DeleteVictim(id);
                TempData["SuccessMessage"] = "Victim deleted successfully.";
            }
            catch (VictimNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}