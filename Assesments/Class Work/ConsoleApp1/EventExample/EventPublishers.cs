using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.EventExample
{
    public delegate void myDelegate(string message);
    internal class EventPublishers
    {
        public event myDelegate myEvent;

        public void myMethod(string message)
        {
            myEvent?.Invoke(message);
        }
    }
}
