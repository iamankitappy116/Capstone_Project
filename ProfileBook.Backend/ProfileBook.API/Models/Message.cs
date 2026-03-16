using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileBook.API.Models;

public class Message
{
  [Key]
  public int MessageId { get; set; }

  [Required]
  public string MessageContent { get; set; } = string.Empty;

  public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

  public int SenderId { get; set; }
  public User? Sender { get; set; }

  public int ReceiverId { get; set; }
  public User? Receiver { get; set; }
}

