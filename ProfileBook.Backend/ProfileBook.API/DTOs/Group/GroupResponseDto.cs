public class GroupResponseDto
{
    public int GroupId { get; set; }

    public string GroupName { get; set; } = string.Empty;

    public string? Description { get; set; }
    public string? Category { get; set; }
    public int MemberCount { get; set; }
    public int PostCount { get; set; }
    public int CreatedByUserId { get; set; }
}
