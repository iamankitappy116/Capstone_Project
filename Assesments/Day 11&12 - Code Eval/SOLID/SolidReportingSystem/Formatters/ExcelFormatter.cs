using SolidReportingSystem.Interfaces;

namespace SolidReportingSystem.Formatters
{
    public class ExcelFormatter : IReportFormatter
    {
        public string Format(string content)
        {
            return $"EXCEL FORMAT: {content}";
        }
    }
}