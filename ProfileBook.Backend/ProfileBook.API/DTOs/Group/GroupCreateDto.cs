public class GroupCreateDto
{
    public string GroupName { get; set; } = string.Empty;

    public string? Description { get; set; }
    public string? Category { get; set; }
    public int CreatedByUserId { get; set; }
    public System.Collections.Generic.List<int> MemberIds { get; set; } = new System.Collections.Generic.List<int>();
}
