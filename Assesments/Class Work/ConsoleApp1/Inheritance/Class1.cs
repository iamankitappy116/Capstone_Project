using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Inheritance
{
    internal class Class1: Inheritance1
    {
        public void Test3()
        {
            Console.WriteLine("Base class method 3.");
        }
        static void Main(string[] args)
        {
            Class1 c1 = new Class1();
            c1.Test1();
            c1.Test2();
            c1.Test3();
        }
    }
}
