using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Crime.Models;
using Crime.DAO;
using Crime.Exceptions;

namespace Crime.Controllers
{
    public class EvidenceController : Controller
    {
        private readonly ICrimeService _crimeService;

        public EvidenceController(ICrimeService crimeService)
        {
            _crimeService = crimeService;
        }

        // GET: Evidence
        public IActionResult Index()
        {
            var evidences = _crimeService.GetAllEvidences();
            return View(evidences);
        }

        // GET: Evidence/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var evidence = _crimeService.GetEvidenceById(id.Value);
                return View(evidence);
            }
            catch (EvidenceNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Evidence/Create
        public IActionResult Create()
        {
            ViewData["IncidentId"] = new SelectList(
                _crimeService.GetAllIncidents(),
                "IncidentId",
                "IncidentId");

            return View();
        }

        // POST: Evidence/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Evidence evidence)
        {
            if (ModelState.IsValid)
            {
                _crimeService.CreateEvidence(evidence);
                TempData["SuccessMessage"] = "Evidence created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["IncidentId"] = new SelectList(
                _crimeService.GetAllIncidents(),
                "IncidentId",
                "IncidentId",
                evidence.IncidentId);

            return View(evidence);
        }

        // GET: Evidence/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var evidence = _crimeService.GetEvidenceById(id.Value);

                ViewData["IncidentId"] = new SelectList(
                    _crimeService.GetAllIncidents(),
                    "IncidentId",
                    "IncidentId",
                    evidence.IncidentId);

                return View(evidence);
            }
            catch (EvidenceNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Evidence/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Evidence evidence)
        {
            if (id != evidence.EvidenceId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _crimeService.UpdateEvidence(evidence);
                    TempData["SuccessMessage"] = "Evidence updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (EvidenceNotFoundException ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(evidence);
        }

        // GET: Evidence/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var evidence = _crimeService.GetEvidenceById(id.Value);
                return View(evidence);
            }
            catch (EvidenceNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Evidence/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _crimeService.DeleteEvidence(id);
                TempData["SuccessMessage"] = "Evidence deleted successfully.";
            }
            catch (EvidenceNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}