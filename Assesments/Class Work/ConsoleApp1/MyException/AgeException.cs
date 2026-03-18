using System;


namespace ConsoleApp1.Models
{
    public class AgeException: Exception
    {
        public AgeException(string message): base(message) 
        {

        } 
    }
}