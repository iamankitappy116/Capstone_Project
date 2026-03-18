using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.DTOs
{
    public class BookResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public string? ISBN { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
    }

    public class CreateBookDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        [Range(1000, 2100, ErrorMessage = "Publication year must be between 1000 and 2100.")]
        public int PublicationYear { get; set; }

        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN must be 10–13 characters.")]
        public string? ISBN { get; set; }

        [Required(ErrorMessage = "AuthorId is required.")]
        public int AuthorId { get; set; }
    }

    public class UpdateBookDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        [Range(1000, 2100, ErrorMessage = "Publication year must be between 1000 and 2100.")]
        public int PublicationYear { get; set; }

        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN must be 10–13 characters.")]
        public string? ISBN { get; set; }

        [Required(ErrorMessage = "AuthorId is required.")]
        public int AuthorId { get; set; }
    }
    public class AuthorResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public int BookCount { get; set; }
    }

    public class CreateAuthorDto
    {
        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Bio { get; set; }
    }

    public class UpdateAuthorDto
    {
        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Bio { get; set; }
    }
}
