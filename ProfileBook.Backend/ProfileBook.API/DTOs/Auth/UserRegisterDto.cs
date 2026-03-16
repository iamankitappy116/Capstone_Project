using System.ComponentModel.DataAnnotations;

namespace ProfileBook.API.DTOs.Auth;

public class UserRegisterDto
{
  [Required]
  [MaxLength(50)]
  public string Username { get; set; } = string.Empty;

  [Required]
  [EmailAddress]
  public string Email { get; set; } = string.Empty;

  [Required]
  [MinLength(6)]
  public string? Password { get; set; }
}
