using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreAPI.Models
{
    
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Range(1000, 2100, ErrorMessage = "Publication year must be a valid year (1000–2100).")]
        public int PublicationYear { get; set; }

        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 13 characters.")]
        public string? ISBN { get; set; }

        [Required(ErrorMessage = "AuthorId is required.")]
        public int AuthorId { get; set; }

        // Navigation property
        [ForeignKey("AuthorId")]
        public Author? Author { get; set; }
    }
}
