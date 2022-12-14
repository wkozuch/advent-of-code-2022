using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    class Day14
    {
        static void Main(string[] args)
        {
            var part2 = true;
            var lines = System.IO.File.ReadAllLines(@"Datasets\day14.txt");
            var positions = new List<Position>();

            foreach (var line in lines)
            {
                var points = line.Split(" -> ").ToList();

                for (int i = 0; i < points.Count - 1; i++)
                {
                    var point1 = points[i].Split(",").ToArray();
                    var point2 = points[i + 1].Split(",").ToArray();
                    var l = GetPointsOnALine(point1, point2);
                    positions.AddRange(l);
                }
            }

            if (part2)
            {
                var floor = GetPointsOnALine(
                   new[] { $"{positions.Min(x => x.Col) - 200}", $"{positions.Max(x => x.Row) + 2}" },
                   new[] { $"{positions.Max(x => x.Col) + 200}", $"{positions.Max(x => x.Row) + 2}" });
                positions.AddRange(floor);
            }
            var rows = positions.Max(x => x.Row) + 1;
            var columnsMin = positions.Min(x => x.Col);
            var columnsMax = positions.Max(x => x.Col);

            var offset = columnsMax - columnsMin;
            var surface = Enumerable.Range(0, rows).Select(x => Enumerable.Range(0, offset + 1).Select(y => '.').ToArray()).ToArray();
            foreach (var p in positions.Distinct())
            {
                surface[p.Row][p.Col - columnsMin] = p.C;
            }
            //DrawSurface(surface);

            var sand = new Position() { C = 'o', Row = 0, Col = 500 - columnsMin };
            bool landed = true;
            var sandCount = 0;
            while (landed)
            {
                Console.WriteLine();
                if (!DropSand(ref surface, sand)) break;
                sandCount++;
                // DrawSurface(surface);
            }
            Console.WriteLine(sandCount);

        }

        private static bool DropSand(ref char[][] surface, Position sand)
        {
            var landed = false;
            var row = sand.Row;
            var col = sand.Col;
            while (!landed)
            {
                if (surface[sand.Row][sand.Col] == 'o') break;
                if (row + 1 >= surface.Length || col < 1 || col >= surface.First().Length) break;

                if (surface[row + 1][col] != 'o' && surface[row + 1][col] != '#')
                {
                    //free fall down
                    row++;
                    continue;
                }
                if (surface[row + 1][col] == 'o' || surface[row + 1][col] == '#')
                {

                    if (row > surface.Length || col < 1) break;
                    //fall left if possible or rigth 
                    if (surface[row + 1][col - 1] != 'o' && surface[row + 1][col - 1] != '#')
                    {
                        col--;
                        continue;
                    }

                    if (surface[row + 1][col + 1] != 'o' && surface[row + 1][col + 1] != '#')
                    {
                        col++;
                        continue;
                    }
                }
                landed = true;
            }
            surface[row][col] = 'o';
            return landed;
        }

        private static List<Position> GetPointsOnALine(string[] point1, string[] point2)
        {
            var list = new List<Position>();
            var p1c = int.Parse(point1[0]);
            var p1r = int.Parse(point1[1]);
            var p2c = int.Parse(point2[0]);
            var p2r = int.Parse(point2[1]);
            if (p1r == p2r)
            {
                return Enumerable.Range(Math.Min(p1c, p2c), Math.Abs(p1c - p2c) + 1).Select(x => new Position() { C = '#', Row = p1r, Col = x }).ToList();
            }
            return Enumerable.Range(Math.Min(p1r, p2r), Math.Abs(p1r - p2r) + 1).Select(x => new Position() { C = '#', Row = x, Col = p1c }).ToList();
        }

        private static void DrawSurface(char[][] surface)
        {
            foreach (var line in surface)
            {
                Console.WriteLine(string.Join("", line));
            }
        }

    }
}