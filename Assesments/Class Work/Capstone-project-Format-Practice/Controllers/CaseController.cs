using Microsoft.AspNetCore.Mvc;
using Crime.Models;
using Crime.DAO;
using Crime.Exceptions;

namespace Crime.Controllers
{
    public class CaseController : Controller
    {
        private readonly ICrimeService _crimeService;

        public CaseController(ICrimeService crimeService)
        {
            _crimeService = crimeService;
        }

        // GET: Case
        public IActionResult Index()
        {
            var cases = _crimeService.GetAllCases();
            return View(cases);
        }

        // GET: Case/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var caseDetails = _crimeService.GetCaseDetails(id.Value);
                return View(caseDetails);
            }
            catch (CaseNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Case/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Case/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Case @case)
        {
            if (ModelState.IsValid)
            {
                _crimeService.CreateCase(@case.CaseDescription, new List<int>());
                TempData["SuccessMessage"] = "Case created successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(@case);
        }

        // GET: Case/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var caseDetails = _crimeService.GetCaseDetails(id.Value);
                return View(caseDetails);
            }
            catch (CaseNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Case/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Case @case)
        {
            if (id != @case.CaseId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _crimeService.UpdateCaseDetails(@case);
                    TempData["SuccessMessage"] = "Case updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (CaseNotFoundException ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(@case);
        }

        // GET: Case/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var caseDetails = _crimeService.GetCaseDetails(id.Value);
                return View(caseDetails);
            }
            catch (CaseNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Case/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _crimeService.DeleteCase(id);
                TempData["SuccessMessage"] = "Case deleted successfully.";
            }
            catch (CaseNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}