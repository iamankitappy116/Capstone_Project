using SolidReportingSystem.Interfaces;
using SolidReportingSystem.Services;
using SolidReportingSystem.Formatters;
using SolidReportingSystem.Models;
using SolidReportingSystem.Documents;

namespace SolidReportingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // LSP + DIP
            Report report = new SalesReport();

            IReportGenerator generator = new ReportGenerator(report);
            IReportSaver saver = new ReportSaver();
            IReportFormatter formatter = new PdfFormatter();

            var service = new ReportService(generator, saver, formatter);
            service.ProcessReport();

            Console.WriteLine("Report processed successfully!");

            // Factory Pattern Demo
            var document = DocumentFactory.CreateDocument("pdf");
            document.Print();
        }
    }
}