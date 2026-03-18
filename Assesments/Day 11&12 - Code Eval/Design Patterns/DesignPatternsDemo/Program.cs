using System;
using DesignPatternsDemo.Factory;
using DesignPatternsDemo.Interfaces;

namespace DesignPatternsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Factory Pattern Demo");

            IDocument pdf = DocumentFactory.CreateDocument("pdf");
            pdf.Create();

            IDocument word = DocumentFactory.CreateDocument("word");
            word.Create();

            Console.ReadLine();
        }
    }
}