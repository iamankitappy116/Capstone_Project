using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Crime.Models;
using Crime.DAO;
using Crime.Exceptions;

namespace Crime.Controllers
{
    public class ReportController : Controller
    {
        private readonly ICrimeService _crimeService;

        public ReportController(ICrimeService crimeService)
        {
            _crimeService = crimeService;
        }

        // GET: Report
        public IActionResult Index()
        {
            var reports = _crimeService.GetAllReports();
            return View(reports);
        }

        // GET: Report/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var report = _crimeService.GetReportById(id.Value);
                return View(report);
            }
            catch (ReportNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Report/Create
        public IActionResult Create()
        {
            ViewData["IncidentId"] = new SelectList(
                _crimeService.GetAllIncidents(),
                "IncidentId",
                "IncidentId");

            ViewData["ReportingOfficerId"] = new SelectList(
                _crimeService.GetAllOfficers(),
                "OfficerId",
                "OfficerId");

            return View();
        }

        // POST: Report/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Report report)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _crimeService.CreateReport(report);
                    TempData["SuccessMessage"] = "Report created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidDateException ex)
                {
                    ModelState.AddModelError("ReportDate", ex.Message);
                }
            }

            ViewData["IncidentId"] = new SelectList(
                _crimeService.GetAllIncidents(),
                "IncidentId",
                "IncidentId",
                report.IncidentId);

            ViewData["ReportingOfficerId"] = new SelectList(
                _crimeService.GetAllOfficers(),
                "OfficerId",
                "OfficerId",
                report.ReportingOfficerId);

            return View(report);
        }

        // GET: Report/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var report = _crimeService.GetReportById(id.Value);

                ViewData["IncidentId"] = new SelectList(
                    _crimeService.GetAllIncidents(),
                    "IncidentId",
                    "IncidentId",
                    report.IncidentId);

                ViewData["ReportingOfficerId"] = new SelectList(
                    _crimeService.GetAllOfficers(),
                    "OfficerId",
                    "OfficerId",
                    report.ReportingOfficerId);

                return View(report);
            }
            catch (ReportNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Report/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Report report)
        {
            if (id != report.ReportId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _crimeService.UpdateReport(report);
                    TempData["SuccessMessage"] = "Report updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (ReportNotFoundException ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidDateException ex)
                {
                    ModelState.AddModelError("ReportDate", ex.Message);
                }
            }

            return View(report);
        }

        // GET: Report/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var report = _crimeService.GetReportById(id.Value);
                return View(report);
            }
            catch (ReportNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Report/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _crimeService.DeleteReport(id);
                TempData["SuccessMessage"] = "Report deleted successfully.";
            }
            catch (ReportNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}