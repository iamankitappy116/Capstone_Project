using System;
using DesignPatternsDemo.Interfaces;

namespace DesignPatternsDemo.Models
{
    public class WordDocument : IDocument
    {
        public void Create()
        {
            Console.WriteLine("Word Document Created.");
        }
    }
}