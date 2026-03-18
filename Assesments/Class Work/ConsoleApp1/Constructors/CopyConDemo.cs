using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Constructors
{
    class CopyConDemo
    {
        int x;
        public CopyConDemo(int i)
        {
            x = i;
            Console.WriteLine("Parameterized Constructor is called: " + i);
        }

        public CopyConDemo(CopyConDemo obj)
        {
            x = obj.x;
            Console.WriteLine("Copy Constructor is called: " + x);
        }

        public void Display()
        {
            Console.WriteLine("Value of x is: " + x);
        }

        static void Main(string[] args) 
        {
            CopyConDemo obj1 = new CopyConDemo(5);
            CopyConDemo obj2 = new CopyConDemo(10);
            CopyConDemo obj3 = new CopyConDemo(obj1);
        }
    }
}
