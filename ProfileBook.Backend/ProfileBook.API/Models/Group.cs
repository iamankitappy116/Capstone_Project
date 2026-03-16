using System.ComponentModel.DataAnnotations;

namespace ProfileBook.API.Models;

public class Group
{
    [Key]
    public int GroupId { get; set; }

    [Required]
    public string GroupName { get; set; } = string.Empty;

    public string? Description { get; set; }
    public string? Category { get; set; }

    public int CreatedByUserId { get; set; }

    public User? CreatedByUser { get; set; }

    public List<GroupMember> Members { get; set; } = new();
}

