using BookStoreAPI.Data;
using BookStoreAPI.DTOs;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{

    [ApiController]
    [Route("api/books")]
    [Produces("application/json")]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreContext _context;

        public BooksController(BookStoreContext context)
        {
            _context = context;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BookResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Select(b => MapToDto(b))
                .ToListAsync();

            return Ok(books);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(BookResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book is null)
                return NotFound(new { message = $"Book with ID {id} was not found." });

            return Ok(MapToDto(book));
        }

        
        [HttpPost]
        [ProducesResponseType(typeof(BookResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDto dto)
        {
            
            var author = await _context.Authors.FindAsync(dto.AuthorId);
            if (author is null)
                return BadRequest(new { message = $"Author with ID {dto.AuthorId} does not exist." });

            var book = new Book
            {
                Title           = dto.Title,
                PublicationYear = dto.PublicationYear,
                ISBN            = dto.ISBN,
                AuthorId        = dto.AuthorId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

          
            await _context.Entry(book).Reference(b => b.Author).LoadAsync();

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, MapToDto(book));
        }

        
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(BookResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDto dto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book is null)
                return NotFound(new { message = $"Book with ID {id} was not found." });

            var author = await _context.Authors.FindAsync(dto.AuthorId);
            if (author is null)
                return BadRequest(new { message = $"Author with ID {dto.AuthorId} does not exist." });

            book.Title           = dto.Title;
            book.PublicationYear = dto.PublicationYear;
            book.ISBN            = dto.ISBN;
            book.AuthorId        = dto.AuthorId;

            await _context.SaveChangesAsync();

            await _context.Entry(book).Reference(b => b.Author).LoadAsync();
            return Ok(MapToDto(book));
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book is null)
                return NotFound(new { message = $"Book with ID {id} was not found." });

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static BookResponseDto MapToDto(Book book) => new()
        {
            Id              = book.Id,
            Title           = book.Title,
            PublicationYear = book.PublicationYear,
            ISBN            = book.ISBN,
            AuthorId        = book.AuthorId,
            AuthorName      = book.Author?.Name ?? string.Empty
        };
    }
}
