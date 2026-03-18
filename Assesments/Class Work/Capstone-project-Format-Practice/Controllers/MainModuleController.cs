using Crime.DAO;
using Microsoft.AspNetCore.Mvc;

namespace Crime.Controllers
{
    public class MainModuleController : Controller
    {
        private readonly ICrimeService _crimeService;

        public MainModuleController(ICrimeService crimeService)
        {
            _crimeService = crimeService;
        }

        // GET: MainModule/Run
        public IActionResult Run()
        {
            var mainModule = new MainModule(_crimeService);
            var results = mainModule.Run();
            return View(results);
        }
    }
}
