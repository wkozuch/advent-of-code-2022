using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    class Day6
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(@"Datasets\day6.txt");
            var input = lines.Single();
            var codeSearch = false;

            for (int i = 0; i < input.Length; i++)
            {
                var size = codeSearch ? 4 : 14;
                if (input.Skip(i).Take(size).Distinct().Count() != size) continue;
                Console.WriteLine($"Marker: {i + size}");
                break;
            }
        }
    }
}
