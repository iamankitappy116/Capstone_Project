using Crime.DAO;
using Crime.Models;
using Crime.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Crime.Controllers
{
    public class AgencyController : Controller
    {
        private readonly ICrimeService _crimeService;

        public AgencyController(ICrimeService crimeservice)
        {
            _crimeService = crimeservice;
        }

        // GET: Agency
        public IActionResult Index()
        {
            var agencies = _crimeService.GetAllAgencies();
            return View(agencies);
        }

        // GET: Agency/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var agency = _crimeService.GetAgencyById(id.Value);
                return View(agency);
            }
            catch (AgencyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Agency/Create
        public IActionResult Create()
        {
            return View();
        }

        //POST: Agency/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Agency agency)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _crimeService.CreateAgency(agency);
                    TempData["SuccessMessage"] = "Agency created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidPhoneNumberException ex)
                {
                    ModelState.AddModelError("PhoneNumber", ex.Message);
                }
            }
            return View(agency);
        }

        //GET: Agency/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var agency = _crimeService.GetAgencyById(id.Value);
                return View(agency);
            }
            catch (AgencyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Agency/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("AgencyId,AgencyName,Jurisdiction,PhoneNumber")] Agency agency)
        {
            if (id != agency.AgencyId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _crimeService.UpdateAgency(agency);
                    TempData["SuccessMessage"] = "Agency updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (AgencyNotFoundException ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidPhoneNumberException ex)
                {
                    ModelState.AddModelError("PhoneNumber", ex.Message);
                }
            }
            return View(agency);
        }

        // GET: Agency/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var agency = _crimeService.GetAgencyById(id.Value);
                return View(agency);
            }
            catch (AgencyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Agency/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _crimeService.DeleteAgency(id);
                TempData["SuccessMessage"] = "Agency deleted successfully.";
            }
            catch (AgencyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}