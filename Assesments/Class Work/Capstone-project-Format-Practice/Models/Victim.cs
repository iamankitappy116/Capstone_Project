using System.ComponentModel.DataAnnotations;

namespace Crime.Models
{
    public class Victim
    {
        public int VictimId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string? PhoneNumber { get; set; }

        // Navigation
        public ICollection<Incident>? Incidents { get; set; }
    }
}