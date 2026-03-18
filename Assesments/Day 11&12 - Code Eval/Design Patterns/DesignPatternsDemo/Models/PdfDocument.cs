using System;
using DesignPatternsDemo.Interfaces;

namespace DesignPatternsDemo.Models
{
    public class PdfDocument : IDocument
    {
        public void Create()
        {
            Console.WriteLine("PDF Document Created.");
        }
    }
}