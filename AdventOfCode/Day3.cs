using System;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    class Day3
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(@"Datasets\day3.txt");
            var score = 0d;
            foreach (var line in lines)
            {
                var size = line.Count();
                var compartment1 = line.Take(size / 2).ToList();
                var compartment2 = line.Skip(size / 2).Take(size/2).ToList();
                var sharedItem = compartment1.Intersect(compartment2).Single();
                var priorioty = char.IsUpper(sharedItem) ? sharedItem - 'A' + 27 : sharedItem - 'a' + 1;
                score += priorioty;
                Console.WriteLine($"Compartment #1: {string.Join("", compartment1)} Compartment #2: {string.Join("", compartment2)} Intersect: {sharedItem} Priority: {priorioty}");

            }
            Console.WriteLine($"Score {score}");
            
            score = 0;
            for (int i = 0; i < lines.Length; i+=3)
            {
                var compartment1 = lines[i];
                var compartment2 = lines[i+1];
                var compartment3 = lines[i+2];
                var sharedItem = compartment1.Intersect(compartment2).Intersect(compartment3).Single();
                var priorioty = char.IsUpper(sharedItem) ? sharedItem - 'A' + 27 : sharedItem - 'a' + 1;
                score += priorioty;
                Console.WriteLine($"Compartment #1: {string.Join("", compartment1)} Compartment #2: {string.Join("", compartment2)} Compartment #3: {string.Join("", compartment3)}  Intersect: {sharedItem} Priority: {priorioty}");

            }
            Console.WriteLine($"Score {score}");
        }

    }
}
