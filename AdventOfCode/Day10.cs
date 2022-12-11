using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    class Day10
    {
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines(@"Datasets\day10.txt");
            var cycle = 0;
            var X = 1;
            var sum = 0;
            var i = 0;
            var crt = "";
            var spriteClean = "........................................";
            var sprite = ReplaceAt(spriteClean, X);
            foreach (var line in lines)
            {
                var inst = line.Split(" ");
                if (line.Contains("noop"))
                {
                    // cycle++;
                    // CheckCycle(cycle, X, ref sum, ref i);

                    crt = crt + sprite[cycle % 40];
                    cycle++;
                    crt = UpdateCrt(cycle, crt);
                }
                else
                {
                    // cycle++;
                    // CheckCycle(cycle, X, ref sum, ref i);
                    // cycle++;
                    // CheckCycle(cycle, X, ref sum, ref i);
                    // X += int.Parse(inst[1]);

                    crt = crt + sprite[cycle % 40];
                    cycle++;
                    crt = UpdateCrt(cycle, crt);

                    crt = crt + sprite[cycle % 40];
                    cycle++;
                    crt = UpdateCrt(cycle, crt);
                    X += int.Parse(inst[1]);
                    sprite = ReplaceAt(spriteClean, X);
                }

            }
            //Console.WriteLine($"{sum}");
        }

        private static void CheckCycle(int cycle, int X, ref int sum, ref int i)
        {
            if (cycle % (20 + i * 40) == 0)
            {
                sum += cycle * X;
                i++;
                Console.WriteLine($"Cycle {cycle}: Strength : {cycle * X}");
            }
        }

        private static string UpdateCrt(int cycle, string crt)
        {
            if (cycle % 40 == 0)
            {
                Console.WriteLine($"{crt}");
                crt = "";
            }
            return crt;
        }

        private static string ReplaceAt(string input, int index)
        {
            char[] chars = input.ToCharArray();
            if (index >= 1) chars[index - 1] = '#';
            if (index >= 0 && index < input.Length) chars[index] = '#';
            if (index >= -1 && index < input.Length - 1) chars[index + 1] = '#';
            return new string(chars);
        }
    }
}