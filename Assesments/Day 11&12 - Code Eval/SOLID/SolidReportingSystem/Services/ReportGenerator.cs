using SolidReportingSystem.Interfaces;
using SolidReportingSystem.Models;

namespace SolidReportingSystem.Services
{
    public class ReportGenerator : IReportGenerator
    {
        private readonly Report _report;

        public ReportGenerator(Report report)
        {
            _report = report;
        }

        public string GenerateReport()
        {
            return _report.GenerateContent();
        }
    }
}