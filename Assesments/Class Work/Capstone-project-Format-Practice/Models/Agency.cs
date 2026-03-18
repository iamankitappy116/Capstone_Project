using System.ComponentModel.DataAnnotations;

namespace Crime.Models
{
    public class Agency
    {
        public int AgencyId { get; set; }

        [Required(ErrorMessage = "Agency name is required.")]
        [StringLength(100, ErrorMessage = "Agency name cannot exceed 100 characters.")]
        public string? AgencyName { get; set; }

        [Required(ErrorMessage = "Jurisdiction is required.")]
        public string? Jurisdiction { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string? PhoneNumber { get; set; }

        // Navigation
        public ICollection<Officer>? Officers { get; set; }
        public ICollection<Incident>? Incidents { get; set; }
    }
}