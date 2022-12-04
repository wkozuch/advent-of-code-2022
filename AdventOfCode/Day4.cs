using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    class Day4
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(@"Datasets\day4.txt");
            var count = 0;
            foreach (var line in lines)
            {
                var pair = line.Split(",");
                var assignment1 = Day4.ExpandSection(pair.First());
                var assignment2 = Day4.ExpandSection(pair.Last());
                // if( !assignment1.Except(assignment2).Any() || !assignment2.Except(assignment1).Any()) count++; //Task #1
                if (assignment1.Intersect(assignment2).Any() || assignment2.Intersect(assignment1).Any()) count++;
                Console.WriteLine($"Assignment #1: {string.Join("", assignment1)} Assignment #2: {string.Join("", assignment2)}");

            }
            Console.WriteLine($"Count {count}");
        }

        static List<int> ExpandSection(string assignment)
        {
            var start = int.Parse(assignment.Split("-").First());
            var end = int.Parse(assignment.Split("-").Last());
            var expanded = new List<int>();
            for (int i = start; i < end + 1; i++)
            {
                expanded.Add(i);
            }
            return expanded;
        }

    }
}
