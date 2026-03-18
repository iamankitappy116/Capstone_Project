using System.ComponentModel.DataAnnotations;

namespace Crime.Models
{
    public class Report
    {
        public int ReportId { get; set; }

        [Required(ErrorMessage = "Report date is required.")]
        public DateTime ReportDate { get; set; }

        [Required(ErrorMessage = "Report details are required.")]
        public string? ReportDetails { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string? Status { get; set; }

        // FK
        public int IncidentId { get; set; }
        public Incident? Incident { get; set; }

        public int ReportingOfficerId { get; set; }
        public Officer? ReportingOfficer { get; set; }
    }
}