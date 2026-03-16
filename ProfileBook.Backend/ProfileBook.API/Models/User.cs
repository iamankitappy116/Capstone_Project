using System.ComponentModel.DataAnnotations;

namespace ProfileBook.API.Models;

public class User
{
  [Key]
  public int UserId { get; set; }

  [Required]
  [MaxLength(50)]
  public string Username { get; set; } = string.Empty;

  [Required]
  [EmailAddress]
  public string Email { get; set; } = string.Empty;

  [Required]
  public byte[] PasswordHash { get; set; }

  [Required]
  public byte[] PasswordSalt { get; set; }

  [Required]
  public string Role { get; set; } = "User";

  public string? ProfileImage { get; set; }

  [MaxLength(500)]
  public string? Bio { get; set; }

  [MaxLength(100)]
  public string? Location { get; set; }

  public int FollowerCount { get; set; } = 0;
  
  public int FollowingCount { get; set; } = 0;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public List<Post> Posts { get; set; } = new List<Post>();
  public List<Comment> Comments { get; set; } = new List<Comment>();
  public List<Like> Likes { get; set; } = new List<Like>();
}
