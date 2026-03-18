using Crime.DAO;
using Crime.Data;
using Crime.Models;
using Crime.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Crime.Controllers
{
    public class OfficersController : Controller
    {
        private readonly ICrimeService _crimeService;
        private readonly CrimeDbContext _context;

        public OfficersController(ICrimeService crimeService, CrimeDbContext context)
        {
            _crimeService = crimeService;
            _context = context;
        }

        // GET: Officers
        public IActionResult Index()
        {
            var officers = _crimeService.GetAllOfficers();
            return View(officers);
        }

        // GET: Officers/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                var officer = _crimeService.GetOfficerById(id);
                return View(officer);
            }
            catch (OfficerNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Officers/Create
        public IActionResult Create()
        {
            ViewBag.AgencyId = new SelectList(_context.Agencies, "AgencyId", "AgencyName");
            return View();
        }

        // POST: Officers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Officer officer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _crimeService.CreateOfficer(officer);
                    TempData["SuccessMessage"] = "Officer created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidPhoneNumberException ex)
                {
                    ModelState.AddModelError("PhoneNumber", ex.Message);
                }
            }

            ViewBag.AgencyId = new SelectList(_context.Agencies, "AgencyId", "AgencyName", officer.AgencyId);
            return View(officer);
        }

        // GET: Officers/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                var officer = _crimeService.GetOfficerById(id);
                ViewBag.AgencyId = new SelectList(_context.Agencies, "AgencyId", "AgencyName", officer.AgencyId);
                return View(officer);
            }
            catch (OfficerNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Officers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Officer officer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _crimeService.UpdateOfficer(officer);
                    TempData["SuccessMessage"] = "Officer updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (OfficerNotFoundException ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidPhoneNumberException ex)
                {
                    ModelState.AddModelError("PhoneNumber", ex.Message);
                }
            }

            ViewBag.AgencyId = new SelectList(_context.Agencies, "AgencyId", "AgencyName", officer.AgencyId);
            return View(officer);
        }

        // GET: Officers/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                var officer = _crimeService.GetOfficerById(id);
                return View(officer);
            }
            catch (OfficerNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Officers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _crimeService.DeleteOfficer(id);
                TempData["SuccessMessage"] = "Officer deleted successfully.";
            }
            catch (OfficerNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
