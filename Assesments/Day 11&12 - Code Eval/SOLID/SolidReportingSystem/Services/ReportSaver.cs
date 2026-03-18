using SolidReportingSystem.Interfaces;
using System.IO;

namespace SolidReportingSystem.Services
{
    public class ReportSaver : IReportSaver
    {
        public void Save(string content)
        {
            File.WriteAllText("report.txt", content);
        }
    }
}