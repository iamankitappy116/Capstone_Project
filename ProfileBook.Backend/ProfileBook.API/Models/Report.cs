using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileBook.API.Models;

public class Report
{
  [Key]
  public int ReportId { get; set; }

  [Required]
  public string Reason { get; set; } = string.Empty;
  public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

  public int ReportedUserId { get; set; }
  public User? ReportedUser { get; set; }

  public int ReportingUserId { get; set; }
  public User? ReportingUser { get; set; }
}

