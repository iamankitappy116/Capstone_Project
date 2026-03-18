using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Constructors
{
    class NeedOfConstructor
    {
        public int x = 100;
    }

    class NeedOfCon
    {
        public int x;
        public NeedOfCon(int x) 
        {
            this.x = x;
        }
    }
    class TestClasses
    {
        static void Main(string[] args)
        {
            NeedOfConstructor nc1 = new NeedOfConstructor();
            NeedOfConstructor nc2 = new NeedOfConstructor();
            NeedOfConstructor nc3 = new NeedOfConstructor();
            Console.WriteLine(nc1.x + " " + nc2.x + " " + nc3.x);

            NeedOfCon obj1 = new NeedOfCon(15);
            NeedOfCon obj2 = new NeedOfCon(17);
            NeedOfCon obj3 = new NeedOfCon(16);
            Console.WriteLine(obj1.x + " " + obj2.x + " " + obj3.x);

        }
    }
}
