using System.ComponentModel.DataAnnotations;

namespace Crime.Models
{
    public class Officer
    {
        public int OfficerId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Badge number is required.")]
        public string? BadgeNumber { get; set; }

        [Required(ErrorMessage = "Rank is required.")]
        public string? Rank { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string? PhoneNumber { get; set; }

        // FK
        public int AgencyId { get; set; }
        public Agency? Agency { get; set; }

        // Navigation
        public ICollection<Report>? Reports { get; set; }
    }
}