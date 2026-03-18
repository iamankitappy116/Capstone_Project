using Microsoft.AspNetCore.Mvc;

namespace RoughProject.Controllers
{
    public class AgencyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
