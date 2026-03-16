namespace ProfileBook.API.DTOs.Report
{
    public class ReportCreateDto
    {
        public int ReportingUserId { get; set; }

        public int ReportedUserId { get; set; }

        public string Reason { get; set; } = string.Empty;
    }
}
