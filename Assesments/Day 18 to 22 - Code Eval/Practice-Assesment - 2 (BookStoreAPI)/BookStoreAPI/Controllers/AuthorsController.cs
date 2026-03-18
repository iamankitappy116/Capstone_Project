using BookStoreAPI.Data;
using BookStoreAPI.DTOs;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{
  
    [ApiController]
    [Route("api/authors")]
    [Produces("application/json")]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreContext _context;

        public AuthorsController(BookStoreContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AuthorResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _context.Authors
                .Include(a => a.Books)
                .Select(a => MapToDto(a))
                .ToListAsync();

            return Ok(authors);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AuthorResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author is null)
                return NotFound(new { message = $"Author with ID {id} was not found." });

            return Ok(MapToDto(author));
        }

   
        [HttpPost]
        [ProducesResponseType(typeof(AuthorResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto dto)
        {
            var author = new Author
            {
                Name = dto.Name,
                Bio  = dto.Bio
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, MapToDto(author));
        }

        
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(AuthorResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] UpdateAuthorDto dto)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author is null)
                return NotFound(new { message = $"Author with ID {id} was not found." });

            author.Name = dto.Name;
            author.Bio  = dto.Bio;

            await _context.SaveChangesAsync();

            return Ok(MapToDto(author));
        }

        
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author is null)
                return NotFound(new { message = $"Author with ID {id} was not found." });

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{authorId:int}/books")]
        [ProducesResponseType(typeof(IEnumerable<BookResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBooksByAuthor(int authorId)
        {
            
            var authorExists = await _context.Authors.AnyAsync(a => a.Id == authorId);
            if (!authorExists)
                return NotFound(new { message = $"Author with ID {authorId} was not found." });

            var books = await _context.Books
                .Include(b => b.Author)
                .Where(b => b.AuthorId == authorId)
                .Select(b => new BookResponseDto
                {
                    Id              = b.Id,
                    Title           = b.Title,
                    PublicationYear = b.PublicationYear,
                    ISBN            = b.ISBN,
                    AuthorId        = b.AuthorId,
                    AuthorName      = b.Author!.Name
                })
                .ToListAsync();

            return Ok(books);
        }

    
        private static AuthorResponseDto MapToDto(Author author) => new()
        {
            Id        = author.Id,
            Name      = author.Name,
            Bio       = author.Bio,
            BookCount = author.Books?.Count ?? 0
        };
    }
}
