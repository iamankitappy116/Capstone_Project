using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Crime.Models;
using Crime.DAO;

namespace Crime.Controllers;

public class HomeController : Controller
{
    private readonly ICrimeService _crimeService;

    public HomeController(ICrimeService crimeService)
    {
        _crimeService = crimeService;
    }

    public IActionResult Index()
    {
        ViewBag.IncidentCount = _crimeService.GetAllIncidents().Count();
        ViewBag.CaseCount = _crimeService.GetAllCases().Count();
        ViewBag.OfficerCount = _crimeService.GetAllOfficers().Count();
        ViewBag.SuspectCount = _crimeService.GetAllSuspects().Count();
        ViewBag.VictimCount = _crimeService.GetAllVictims().Count();
        ViewBag.AgencyCount = _crimeService.GetAllAgencies().Count();
        ViewBag.EvidenceCount = _crimeService.GetAllEvidences().Count();
        ViewBag.ReportCount = _crimeService.GetAllReports().Count();

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
