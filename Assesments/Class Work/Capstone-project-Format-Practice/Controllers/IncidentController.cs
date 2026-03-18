using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Crime.Data;
using Crime.Models;
using Crime.DAO;
using Crime.Exceptions;

namespace Crime.Controllers
{
    public class IncidentController : Controller
    {
        private readonly ICrimeService _crimeService;
        private readonly CrimeDbContext _context;

        public IncidentController(ICrimeService crimeService, CrimeDbContext context)
        {
            _crimeService = crimeService;
            _context = context;
        }

        // GET: Incident
        public IActionResult Index()
        {
            var incidents = _crimeService.GetAllIncidents();
            return View(incidents);
        }

        // GET: Incident/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var incident = _crimeService.GetIncidentById(id.Value);
                return View(incident);
            }
            catch (IncidentNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Incident/Create
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        // POST: Incident/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Incident incident)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _crimeService.CreateIncident(incident);
                    TempData["SuccessMessage"] = "Incident created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidDateException ex)
                {
                    ModelState.AddModelError("IncidentDate", ex.Message);
                }
            }

            LoadDropdowns();
            return View(incident);
        }

        // GET: Incident/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var incident = _crimeService.GetIncidentById(id.Value);
                LoadDropdowns();
                return View(incident);
            }
            catch (IncidentNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Incident/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Incident incident)
        {
            if (id != incident.IncidentId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _crimeService.UpdateIncident(incident);
                    TempData["SuccessMessage"] = "Incident updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (IncidentNotFoundException ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return RedirectToAction(nameof(Index));
                }
            }

            LoadDropdowns();
            return View(incident);
        }

        // GET: Incident/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var incident = _crimeService.GetIncidentById(id.Value);
                return View(incident);
            }
            catch (IncidentNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Incident/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _crimeService.DeleteIncident(id);
                TempData["SuccessMessage"] = "Incident deleted successfully.";
            }
            catch (IncidentNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // 🔹 Dropdown loader (UI support only — acceptable to use DbContext here)
        private void LoadDropdowns()
        {
            ViewData["AgencyId"] = new SelectList(_context.Agencies, "AgencyId", "AgencyName");
            ViewData["SuspectId"] = new SelectList(_context.Suspects, "SuspectId", "FirstName");
            ViewData["VictimId"] = new SelectList(_context.Victims, "VictimId", "FirstName");
        }
    }
}