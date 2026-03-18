using SolidReportingSystem.Interfaces;

namespace SolidReportingSystem.Services
{
    public class ReportService
    {
        private readonly IReportGenerator _generator;
        private readonly IReportSaver _saver;
        private readonly IReportFormatter _formatter;

        public ReportService(IReportGenerator generator,
                             IReportSaver saver,
                             IReportFormatter formatter)
        {
            _generator = generator;
            _saver = saver;
            _formatter = formatter;
        }

        public void ProcessReport()
        {
            var report = _generator.GenerateReport();
            var formatted = _formatter.Format(report);
            _saver.Save(formatted);
        }
    }
}