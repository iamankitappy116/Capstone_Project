using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Constructors
{
    
    class ConstructorsDemo
    {
        int x;
        static int y;
        static ConstructorsDemo()
        {
            Console.WriteLine("Static constructor is Called");
        }
        public ConstructorsDemo()
        {
            Console.WriteLine("Non-Static constructor is called");
        }

        static void Main(string[] args) 
        {
            Console.WriteLine("Main method is Called");
             ConstructorsDemo cd = new ConstructorsDemo();
             Console.ReadLine();
        }
    }
}
