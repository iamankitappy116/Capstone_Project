using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Constructors
{
    public class Constructor1
    {
        int i; bool b;
        static void Main(string[] args)
        {
            Console.WriteLine("This is Second Main Method");
            Constructor1 c = new Constructor1();
            Console.WriteLine(c.i);
            Console.WriteLine(c.b);
        }
    }
}
