using System.ComponentModel.DataAnnotations;

namespace Crime.Models
{
    public class Evidence
    {
        public int EvidenceId { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Location found is required.")]
        public string? LocationFound { get; set; }

        // FK
        public int IncidentId { get; set; }
        public Incident? Incident { get; set; }
    }
}