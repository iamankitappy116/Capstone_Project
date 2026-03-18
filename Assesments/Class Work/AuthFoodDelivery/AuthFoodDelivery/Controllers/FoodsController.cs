using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.Models;
using AuthFoodDeliveryApp.FoodDeliveryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace FoodDelivery.Controllers
{
    [Route("Foods")]
    [Authorize]
    public class FoodsController : Controller
    {
        private readonly FoodDeliveryDbCOntext _context;
        private readonly IWebHostEnvironment _env;

        public FoodsController(FoodDeliveryDbCOntext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Foods
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var foods = await _context.Foods.Include(f => f.Category).ToListAsync();
            return View(foods);
        }

        // GET: Foods/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var food = await _context.Foods
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (food == null) return NotFound();

            return View(food);
        }

        // GET: Foods/Create
        [HttpGet("Create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.categId = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Foods/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Food food, IFormFile? ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var folder = Path.Combine(_env.WebRootPath, "food-images");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                food.ImagePath = "/food-images/" + fileName;
            }

            if (ModelState.IsValid)
            {
                _context.Foods.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.categId = new SelectList(_context.Categories, "Id", "Name", food.categId);
            return View(food);
        }

        // GET: Foods/Edit/5
        [HttpGet("Edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var food = await _context.Foods.FindAsync(id);
            if (food == null) return NotFound();

            ViewBag.categId = new SelectList(_context.Categories, "Id", "Name", food.categId);
            return View(food);
        }

        // POST: Foods/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Food food, IFormFile? ImageFile)
        {
            if (id != food.Id) return NotFound();

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var folder = Path.Combine(_env.WebRootPath, "food-images");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                food.ImagePath = "/food-images/" + fileName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(food);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(food.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.categId = new SelectList(_context.Categories, "Id", "Name", food.categId);
            return View(food);
        }

        // GET: Foods/Delete/5
        [HttpGet("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var food = await _context.Foods
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (food == null) return NotFound();

            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food != null)
            {
                _context.Foods.Remove(food);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Filter")]
        public IActionResult Filter()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpGet("GetFoodsByCategory/{categoryId}")]
        public async Task<JsonResult> GetFoodsByCategory(int categoryId)
        {
            var foods = await _context.Foods
                .Where(f => f.categId == categoryId)
                .Select(f => new { f.Id, f.Name })
                .ToListAsync();
            return Json(foods);
        }

        private bool FoodExists(int id)
        {
            return _context.Foods.Any(e => e.Id == id);
        }
    }
}