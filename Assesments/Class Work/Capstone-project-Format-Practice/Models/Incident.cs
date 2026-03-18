using System.ComponentModel.DataAnnotations;

namespace Crime.Models
{
    public class Incident
    {
        public int IncidentId { get; set; }

        [Required(ErrorMessage = "Incident type is required.")]
        public string? IncidentType { get; set; }

        [Required(ErrorMessage = "Incident date is required.")]
        public DateTime IncidentDate { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string? Location { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string? Status { get; set; }

        // FK
        public int VictimId { get; set; }
        public Victim? Victim { get; set; }

        public int SuspectId { get; set; }
        public Suspect? Suspect { get; set; }

        public int AgencyId { get; set; }
        public Agency? Agency { get; set; }

        // Navigation
        public ICollection<Evidence>? Evidences { get; set; }
        public ICollection<Report>? Reports { get; set; }
    }
}