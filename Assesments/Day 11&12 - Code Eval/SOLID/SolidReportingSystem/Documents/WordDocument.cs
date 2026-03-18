using System;

namespace SolidReportingSystem.Documents
{
    public class WordDocument : IDocument
    {
        public void Print()
        {
            Console.WriteLine("Printing Word Document");
        }
    }
}