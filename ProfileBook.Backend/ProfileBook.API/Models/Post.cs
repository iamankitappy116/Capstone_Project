using System.ComponentModel.DataAnnotations;

namespace ProfileBook.API.Models;

public class Post
{
  [Key]
  public int PostId { get; set; }

  [Required]
  [MaxLength(500)]
  public string Content { get; set; } = string.Empty;

  public string? MediaUrl { get; set; }
  public string? MediaType { get; set; }

  [Required]
  public string Status { get; set; } = "Pending";

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public int UserId { get; set; }
  public User? User { get; set; }

  public int? GroupId { get; set; }
  public Group? Group { get; set; }

  public List<Comment> Comments { get; set; } = new List<Comment>();
  public List<Like> Likes { get; set; } = new List<Like>();
}

