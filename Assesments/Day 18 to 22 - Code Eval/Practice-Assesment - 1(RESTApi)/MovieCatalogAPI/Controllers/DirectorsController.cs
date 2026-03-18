using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCatalogAPI.Data;
using MovieCatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MovieCatalogAPI.Controllers
{
    [Route("api/directors")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DirectorsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDirectors()
        {
            return Ok(await _context.Directors.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateDirector(Director director)
        {
            _context.Directors.Add(director);
            await _context.SaveChangesAsync();
            return Ok(director);
        }

        // GET movies by director

        [HttpGet("{directorId}/movies")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByDirector(int directorId)
        {
            var director = await _context.Directors
                .Include(d => d.Movies)
                .FirstOrDefaultAsync(d => d.Id == directorId);

            if (director == null)
            {
                return NotFound("Director not found");
            }

            return Ok(director.Movies);
        }
    }
}
