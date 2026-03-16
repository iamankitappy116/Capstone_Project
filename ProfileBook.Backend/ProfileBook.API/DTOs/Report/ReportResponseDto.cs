namespace ProfileBook.API.DTOs.Report
{
    public class ReportResponseDto
    {
        public int ReportId { get; set; }

        public int ReportingUserId { get; set; }
        public string? ReportingUserName { get; set; }

        public int ReportedUserId { get; set; }
        public string? ReportedUserName { get; set; }

        public string Reason { get; set; } = string.Empty;
        public string? Severity { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}