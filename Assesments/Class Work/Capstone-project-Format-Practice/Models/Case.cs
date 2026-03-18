using System.ComponentModel.DataAnnotations;

namespace Crime.Models
{
    public class Case
    {
        public int CaseId { get; set; }

        [Required]
        public string CaseDescription { get; set; } = string.Empty;

        // Navigation
        public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    }
}