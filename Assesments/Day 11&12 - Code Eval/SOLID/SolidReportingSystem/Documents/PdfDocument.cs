using System;

namespace SolidReportingSystem.Documents
{
    public class PdfDocument : IDocument
    {
        public void Print()
        {
            Console.WriteLine("Printing PDF Document");
        }
    }
}