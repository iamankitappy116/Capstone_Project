using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters.")]
        public string? Bio { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
