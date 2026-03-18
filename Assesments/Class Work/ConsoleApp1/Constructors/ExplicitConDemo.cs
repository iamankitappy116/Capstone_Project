using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Constructors
{
    public class ExplicitConDemo
    {
        public ExplicitConDemo()
        {
            Console.WriteLine("Contructor is called.");
        }

        static void Main(string[] args)
        {
            ExplicitConDemo obj = new ExplicitConDemo();
            Console.ReadLine();
        }
    }
}
