using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Constructors
{
    class StaticConDemo
    {
        static StaticConDemo()
        {
            //This static constructor will be called before the first instance of StaticConDemo is created
            //It is used to initialize static members of the class or to perform any actions that need to be performed only once
            //It does not take any parameters and cannot be called directly.
            Console.WriteLine("Static constructor called.");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Main method called.");
        }
    }
}
