using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    class Day1
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(@"Datasets\day1.txt");
            var calories = 0.0;
            var i = 0;
            var elfs = new List<double>();
            foreach (var line in lines)
            {
                if (line == "")
                {
                    Console.WriteLine($"Elf #{++i} calories: {calories}");
                    elfs.Add(calories);
                    calories = 0;
                    continue;
                }
                calories += double.Parse(line);
            }
            Console.WriteLine($"Elf #{++i} calories: {calories}");
            elfs.Add(calories);

            var order = elfs.OrderByDescending(x => x).ToList();

            Console.WriteLine($"Max calories: {order.Max()} 1# {order.First()} 2# {order.Skip(1).First()} 3# {order.Skip(2).First()}");
            Console.WriteLine($"Sum of three top calories: {order.First() + order.Skip(1).First() + order.Skip(2).First()}");
        }

    }
}
