using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    class Day15
    {
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines(@"Datasets\day15.txt");
            var positions = new List<Position>();
            var sensorsAndBeacons = new List<Position>();
            int maxLimit = 4000000; //4000000
            var minLimit = 0; //4000000;

            for (int i = minLimit; i <= maxLimit; i++)
            {
                var coverageInRow = new List<(int, int)>();
                foreach (var line in lines)
                {
                    var matches = Regex.Matches(line, @"(?<==)[\w.-]+").Select(x => int.Parse(x.Value)).ToArray();
                    var sensor = new Position { C = 'S', Col = matches[0], Row = matches[1] };
                    var beacon = new Position { C = 'B', Col = matches[2], Row = matches[3] };
                    var distance = Math.Abs(sensor.Row - beacon.Row) + Math.Abs(sensor.Col - beacon.Col);
                    var distanceToRow = Math.Abs(sensor.Row - i);
                    var distanceToColumn = Math.Abs(sensor.Col - i);
                    var columnSpan = distance - distanceToRow;
                    var rowSpan = distance - distanceToColumn;
                    if (distanceToRow > distance) continue;
                    var fromTo = (Math.Min(sensor.Col - columnSpan, sensor.Col + columnSpan), Math.Max(sensor.Col - columnSpan, sensor.Col + columnSpan));
                    coverageInRow.Add(fromTo);
                }

                var rangesInLine = CountLineCoverage(coverageInRow.OrderBy( x => x.Item1).ToList());
                var jointRange = JoinRanges(rangesInLine.OrderBy(x => x.Item1).ToList());

                Console.WriteLine(jointRange.First().Item2 - jointRange.First().Item1);

                if (jointRange.Count == 2)
                {
                    if (jointRange.Last(y => y.Item1 > 0).Item1 - jointRange.First().Item2 == 2)
                    {
                        var x = jointRange.First().Item2 + 1;
                        Console.WriteLine($" Col = {x} Row = {i} Freq = {(double)x * 4000000 + i}");
                    }
                }

            }
        }
        private static List<(int, int)> JoinRanges(List<(int, int)> rangesInLine)
        {
            var joint = new List<(int, int)>();
            for (int i = 0; i < rangesInLine.Count; i++)
            {
                var current = rangesInLine.ElementAt(i);
                if (current == rangesInLine.Last())
                {
                    joint.Add(current);
                    break;
                }

                var next = rangesInLine.ElementAt(i + 1);
                if (rangesInLine.ElementAt(i).Item2 >= rangesInLine.ElementAt(i + 1).Item1 || rangesInLine.ElementAt(i).Item2 + 1 == rangesInLine.ElementAt(i + 1).Item1)
                {
                    joint.Add((current.Item1, next.Item2));
                    i++;
                }
                else
                {
                    joint.Add(current);
                }
            }

            return joint;
        }

        private static List<(int, int)> CountLineCoverage(List<(int, int)> coverageInRow)
        {
            var ranges = new List<(int, int)>();

            foreach (var kvp in coverageInRow.ToList())
            {
                if (ranges.All(x => x.Item1 > kvp.Item1 && x.Item2 < kvp.Item2))
                {
                    ranges.Clear();
                    ranges.Add((kvp.Item1, kvp.Item2));
                    continue;
                } //wider than any

                if (ranges.All(x => kvp.Item2 < x.Item1 || x.Item2 < kvp.Item1))
                {
                    ranges.Add((kvp.Item1, kvp.Item2));
                    continue;
                } //outside current ranges

                if (ranges.Any(x => x.Item1 <= kvp.Item1 && kvp.Item2 <= x.Item2))
                    continue; //narrower range than the already covered

                if (ranges.Any(x => x.Item1 <= kvp.Item1 && kvp.Item1 <= x.Item2))
                {
                    var lowEnds = ranges.Where(x => x.Item1 <= kvp.Item1 && kvp.Item1 <= x.Item2 || kvp.Item1 <= x.Item1 && x.Item2 <= kvp.Item2).ToList();
                    lowEnds.ForEach(x => ranges.Remove(x));
                    ranges.Add((Math.Min(lowEnds.Min(x => x.Item1), kvp.Item1), Math.Max(lowEnds.Max(x => x.Item2), kvp.Item2)));
                    continue;
                }//too narrow on the left

                if (ranges.Any(x => x.Item1 <= kvp.Item2 && kvp.Item2 <= x.Item2))
                {
                    var lowEnds = ranges.Where(x => x.Item1 <= kvp.Item2 && kvp.Item2 <= x.Item2 || kvp.Item1 <= x.Item1 && x.Item2 <= kvp.Item2).ToList();
                    lowEnds.ForEach(x => ranges.Remove(x));
                    ranges.Add((Math.Min(lowEnds.Min(x => x.Item1), kvp.Item1), Math.Max(lowEnds.Max(x => x.Item2), kvp.Item2)));
                    continue;
                }//too narrow on the right

            }
            ranges = ranges.OrderBy(x => x.Item1).ToList();
            return ranges;
        }

        private static void DrawSurface(char[][] surface, int rowsMin)
        {
            var i = rowsMin;
            foreach (var line in surface)
            {
                Console.WriteLine($"{i++:000} " + string.Join("", line));
            }
        }

    }
}