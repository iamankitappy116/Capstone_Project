using System;
using System.ComponentModel;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleAppForLamdaFunc
{
    class Program
    {
        static void Main(string[] args)
        {
            //1.Filtering Operators:
            var numbers1 = new[] { 1, 2, 3, 4, 5, 6 };
            var evenNumbers = numbers1.Where(n => n % 2 == 0);
            foreach (var number in evenNumbers) { 
                Console.WriteLine(number);
            }

            //2.Projection Operators


            //Select
            var numbers2 = new[] { 1, 2, 3 };
            var squaredNumbers = numbers2.Select(n => n * n);


            //Select Man(flattens collection inside a collection (into one single array)
            var words = new[] { "hello", "world" };
            var characters = words.SelectMany(w => w.ToCharArray());
            foreach (var character in characters)
            {
                Console.WriteLine(character);
            }

            //Sorting Operators
            //OrderBy
            var numbers3 = new[] { 5, 1, 4, 2 };
            var sortedNumbers = numbers3.OrderBy(n => n);
            foreach (var number in sortedNumbers)
            {
                Console.WriteLine(number);
            }

            //OrderByDescending
            var sortedDesc = numbers3.OrderByDescending(n => n);
            foreach (var number in sortedDesc)
            {
                Console.WriteLine(number);
            }
            //ThenBy
            var people = new[]
            {
                new { Name = "John", Age = 30 },
                new { Name = "Alice", Age = 25 },
                new { Name = "John", Age = 22 }
            };
            var sortedPeople = people.OrderBy(p => p.Name).ThenBy(p => p.Age);
            foreach (var person in sortedPeople)
            {
                Console.WriteLine($"Name: {person.Name}, Age: {person.Age}");
            }


            //ThenByDescending
            var sortedPeopleDesc = people.OrderBy(p => p.Name).ThenByDescending(p => p.Age);

            foreach (var person in sortedPeopleDesc)
            {
                Console.WriteLine($"Name: {person.Name}, Age: {person.Age}");
            }


            var numbers = new[] { 1, 2, 3, 4 };

            // Count
            var count = numbers.Count();
            Console.WriteLine("Count: " + count);

            // Sum
            var sum = numbers.Sum();
            Console.WriteLine("Sum: " + sum);

            // Average
            var avg = numbers.Average();
            Console.WriteLine("Average: " + avg);

            // Min
            var min = numbers.Min();
            Console.WriteLine("Min: " + min);

            // Max
            var max = numbers.Max();
            Console.WriteLine("Max: " + max);

            // Any
            var hasEven = numbers.Any(n => n % 2 == 0);
            Console.WriteLine("Has Even: " + hasEven);

            // All
            var allPositive = numbers.All(n => n > 0);
            Console.WriteLine("All Positive: " + allPositive);

            // Contains
            var containsThree = numbers.Contains(3);
            Console.WriteLine("Contains 3: " + containsThree);

            Console.WriteLine();

            // Distinct
            var numbersWithDuplicates = new[] { 1, 2, 2, 3, 4, 4 };
            var distinctNumbers = numbersWithDuplicates.Distinct();

            Console.WriteLine("Distinct:");
            foreach (var n in distinctNumbers)
                Console.WriteLine(n);

            Console.WriteLine();

            // Union
            var set1 = new[] { 1, 2, 3 };
            var set2 = new[] { 3, 4, 5 };

            var unionSet = set1.Union(set2);

            Console.WriteLine("Union:");
            foreach (var n in unionSet)
                Console.WriteLine(n);

            Console.WriteLine();

            // Intersect
            var intersectSet = set1.Intersect(set2);

            Console.WriteLine("Intersect:");
            foreach (var n in intersectSet)
                Console.WriteLine(n);

            Console.WriteLine();

            // Except
            var exceptSet = set1.Except(set2);

            Console.WriteLine("Except:");
            foreach (var n in exceptSet)
                Console.WriteLine(n);

            Console.WriteLine();

            // Partitioning Operators

            // Take
            var firstTwo = numbers.Take(2);
            Console.WriteLine("Take(2):");
            foreach (var n in firstTwo)
                Console.WriteLine(n);

            Console.WriteLine();

            // Skip
            var skipTwo = numbers.Skip(2);
            Console.WriteLine("Skip(2):");
            foreach (var n in skipTwo)
                Console.WriteLine(n);

            Console.WriteLine();

            // TakeWhile
            var takeWhileLessThanFour = numbers.TakeWhile(n => n < 4);
            Console.WriteLine("TakeWhile < 4:");
            foreach (var n in takeWhileLessThanFour)
                Console.WriteLine(n);

            Console.WriteLine();

            // SkipWhile
            var skipWhileLessThanFour = numbers.SkipWhile(n => n < 4);
            Console.WriteLine("SkipWhile < 4:");
            foreach (var n in skipWhileLessThanFour)
                Console.WriteLine(n);


            var numbers4 = new[] { 1, 2, 3, 4 };

            // First
            var firstNumber = numbers4.First();
            Console.WriteLine("First: " + firstNumber);

            // FirstOrDefault
            var firstEvenOrDefault = numbers4.FirstOrDefault(n => n % 2 == 0);
            Console.WriteLine("First Even Or Default: " + firstEvenOrDefault);

            // Last
            var lastNumber = numbers4.Last();
            Console.WriteLine("Last: " + lastNumber);

            // LastOrDefault
            var lastEvenOrDefault = numbers4.LastOrDefault(n => n % 2 == 0);
            Console.WriteLine("Last Even Or Default: " + lastEvenOrDefault);

            // Single
            var singleElement = new[] { 42, 42, 45 }.Single(e => e == 42);
            Console.WriteLine("Single: " + singleElement);

            // SingleOrDefault
            var singleOrDefaultElement = new int[] { }.SingleOrDefault();
            Console.WriteLine("SingleOrDefault (empty): " + singleOrDefaultElement);

            // ElementAt
            var secondElement = numbers4.ElementAt(1);
            Console.WriteLine("ElementAt(1): " + secondElement);

            // ElementAtOrDefault
            var outOfRangeElement = numbers4.ElementAtOrDefault(10);
            Console.WriteLine("ElementAtOrDefault(10): " + outOfRangeElement);
        }
    }
}