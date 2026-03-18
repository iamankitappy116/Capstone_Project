using System;

namespace SolidReportingSystem.Documents
{
    public class DocumentFactory
    {
        public static IDocument CreateDocument(string type)
        {
            return type.ToLower() switch
            {
                "pdf" => new PdfDocument(),
                "word" => new WordDocument(),
                _ => throw new ArgumentException("Invalid document type")
            };
        }
    }
}