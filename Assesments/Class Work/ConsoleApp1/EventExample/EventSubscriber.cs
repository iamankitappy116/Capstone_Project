using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.EventExample
{
    internal class EventSubscriber
    {
        public void SubscriberMethod(string message)
        {
            Console.WriteLine($"Message from Publisher: {message}");
        }
    }
}
