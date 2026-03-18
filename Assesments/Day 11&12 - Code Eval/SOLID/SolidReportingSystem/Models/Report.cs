namespace SolidReportingSystem.Models
{
    // Base class (LSP demonstration)
    public abstract class Report
    {
        public string Title { get; set; }

        public abstract string GenerateContent();
    }
}