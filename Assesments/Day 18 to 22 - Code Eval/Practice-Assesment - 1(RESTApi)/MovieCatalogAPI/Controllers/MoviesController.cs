using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalogAPI.Data;
using MovieCatalogAPI.Models;

[Route("api/movies")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly AppDbContext _context;

    public MoviesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/movies
    [HttpGet]
    public async Task<IActionResult> GetMovies()
    {
        return Ok(await _context.Movies.ToListAsync());
    }

    // GET: api/movies/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
            return NotFound(new { message = "Movie not found" });

        return Ok(movie);
    }

    // POST: api/movies
    [HttpPost]
    public async Task<IActionResult> CreateMovie(Movie movie)
    {
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
    }

    // PUT: api/movies/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(int id, Movie movie)
    {
        if (id != movie.Id)
            return BadRequest();

        _context.Entry(movie).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/movies/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
            return NotFound();

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}