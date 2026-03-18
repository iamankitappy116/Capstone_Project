using SolidReportingSystem.Interfaces;

namespace SolidReportingSystem.Formatters
{
    public class PdfFormatter : IReportFormatter
    {
        public string Format(string content)
        {
            return $"PDF FORMAT: {content}";
        }
    }
}