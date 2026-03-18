using Crime.DAO;
using Crime.Models;
using Crime.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Crime.Controllers
{
    public class SuspectsController : Controller
    {
        private readonly ICrimeService _crimeService;

        public SuspectsController(ICrimeService crimeService)
        {
            _crimeService = crimeService;
        }

        // GET: Suspects
        public IActionResult Index()
        {
            var suspects = _crimeService.GetAllSuspects();
            return View(suspects);
        }

        // GET: Suspects/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var suspect = _crimeService.GetSuspectById(id.Value);
                return View(suspect);
            }
            catch (SuspectNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Suspects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suspects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("SuspectId,FirstName,LastName,DateOfBirth,Gender,Address,PhoneNumber")] Suspect suspect)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _crimeService.CreateSuspect(suspect);
                    TempData["SuccessMessage"] = "Suspect created successfully.";
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
            return View(suspect);
        }

        // GET: Suspects/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var suspect = _crimeService.GetSuspectById(id.Value);
                return View(suspect);
            }
            catch (SuspectNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Suspects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("SuspectId,FirstName,LastName,DateOfBirth,Gender,Address,PhoneNumber")] Suspect suspect)
        {
            if (id != suspect.SuspectId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _crimeService.UpdateSuspect(suspect);
                    TempData["SuccessMessage"] = "Suspect updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (SuspectNotFoundException ex)
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

            return View(suspect);
        }

        // GET: Suspects/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var suspect = _crimeService.GetSuspectById(id.Value);
                return View(suspect);
            }
            catch (SuspectNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Suspects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _crimeService.DeleteSuspect(id);
                TempData["SuccessMessage"] = "Suspect deleted successfully.";
            }
            catch (SuspectNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
