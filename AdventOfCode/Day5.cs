using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    class Day5
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(@"Datasets\day5.txt");
            var tops = "";
            var stacks = new List<Stack>();
            // stacks.Add(new Stack(new[] { "Z", "N" }));
            // stacks.Add(new Stack(new[] { "M", "C", "D" }));
            // stacks.Add(new Stack(new[] { "P" }));
            stacks.Add(new Stack(new[] { "G", "D", "V", "Z", "J", "S", "B" }));
            stacks.Add(new Stack(new[] { "Z", "S", "M", "G", "V", "P" }));
            stacks.Add(new Stack(new[] { "C", "L", "B", "S", "W", "T", "Q", "F" }));
            stacks.Add(new Stack(new[] { "H", "J", "G", "W", "M", "R", "V", "Q" }));
            stacks.Add(new Stack(new[] { "C", "L", "S", "N", "F", "M", "D" }));
            stacks.Add(new Stack(new[] { "R", "G", "C", "D" }));
            stacks.Add(new Stack(new[] { "H", "G", "T", "R", "J", "D", "S", "Q" }));
            stacks.Add(new Stack(new[] { "P", "F", "V" }));
            stacks.Add(new Stack(new[] { "D", "R", "S", "T", "J" }));

            foreach (var line in lines)
            {
                var split = line.Split(" ");
                var quantity = int.Parse(split[1]);
                var from = int.Parse(split[3]);
                var to = int.Parse(split[5]);
                var movedItems = new List<string>();

                // for (int i = 0; i < quantity; i++)
                // {
                //     var moveItem = stacks[from - 1].Pop();
                //     stacks[to - 1].Push(moveItem);
                // }

                for (int i = 0; i < quantity; i++)
                {
                    var moveItem = stacks[from - 1].Pop();
                    movedItems.Add((string)moveItem);
                }
                
                movedItems.Reverse();

                foreach (var item in movedItems)
                {
                    stacks[to - 1].Push(item);
                }

                foreach (var stack in stacks)
                {
                    Console.WriteLine($"{string.Join("", stack.ToArray())}");
                }

                Console.WriteLine();
            }
            
            foreach (var stack in stacks)
            {
                // Console.WriteLine($"{string.Join("", stack.ToArray())}");
                tops +=stack.Peek(); 
            }

            Console.WriteLine($"Tops: {tops}");
        }
    }
}
