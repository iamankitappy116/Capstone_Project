using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Constructors
{
    class ParameterizedConDemo
    {
        int x;
        public ParameterizedConDemo(int i)
        {
            x = i;
            Console.WriteLine("Parameterized Constructor is called: " + i);
        }
        public void Display() 
        {
            Console.WriteLine("Value of x is: " + x);
        }   
        static void Main(string[] args) { 
            ParameterizedConDemo obj = new ParameterizedConDemo(5);
            obj.Display();
        }
    }
}
