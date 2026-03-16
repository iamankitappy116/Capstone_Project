using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileBook.API.Models;

public class Comment
{
  [Key]
  public int CommentId { get; set; }

  [Required]
  [MaxLength(300)]
  public string CommentText { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public int UserId { get; set; }
  public User? User { get; set; }
  public int PostId { get; set; }
  public Post? Post { get; set; }

  
}
