using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    class Day9
    {
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines(@"Datasets\day9.txt");
            var size = 5001;
            var surface = Enumerable.Range(0, size).Select(x => Enumerable.Range(0, size).Select(y => '.').ToArray()).ToArray();
            var coloredTiles = Enumerable.Range(0, size).Select(i => Enumerable.Range(0, size).Select(j => '.').ToArray()).ToArray();

            var x = size / 2;
            var y = size / 2;
            var start = new Position { C = 's', Row = x, Col = y };
            var head = new Position { C = 'H', Row = start.Row, Col = start.Col };
            var tail = new Position { C = 'T', Row = start.Row, Col = start.Col };
            var tails = Enumerable.Range(1, 9).Select(x => new Position() { C = char.Parse(x.ToString()), Row = start.Row, Col = start.Col }).ToArray();
            surface[start.Row][start.Col] = start.C;
            surface[tail.Row][tail.Col] = tail.C;
            surface[head.Row][head.Col] = head.C;

            //DrawSurface(surface);
            //Console.WriteLine();

            foreach (var line in lines)
            {
                var direction = line.Split(" ").First();
                var step = int.Parse(line.Split(" ").Last());
                //update visited by tail
                if (surface[tail.Row][tail.Col] != 's') surface[tail.Row][tail.Col] = '#';

                //update head
                for (int i = 0; i < step; i++)
                {
                    surface[head.Row][head.Col] = '.';
                    head = MoveHead(head, direction);
                    surface[head.Row][head.Col] = head.C;
                    //DrawSurface(surface);
                    //Console.WriteLine("Move tail");

                    //update tail
                    surface[tail.Row][tail.Col] = '.';
                    tail = MoveTail(head, tail);
                    surface[tail.Row][tail.Col] = tail.C;
                    //coloredTiles[tail.Row][tail.Col] = '#';

                    //update tails
                    var leader = head;
                    foreach (var t in tails)
                    {
                        surface[t.Row][t.Col] = '.';
                        leader = MoveTail(leader, t);
                        surface[leader.Row][leader.Col] = leader.C;
                        if (leader.C == '9') coloredTiles[leader.Row][leader.Col] = '#';
                    }

                  //  DrawSurface(surface);
                  //  Console.WriteLine();
                }
            }
            
            // DrawSurface(coloredTiles);

            var count = Flatten<char>(coloredTiles).Count(x => x == '#');
            Console.WriteLine(count);
        }

        private static IEnumerable<T> Flatten<T>(T[][] map)
        {
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map[0].Length; col++)
                {
                    yield return map[row][col];
                }
            }
        }

        private static Position MoveTail(Position head, Position tail)
        {
            //is touching bottom? 
            if (tail.Col == head.Col + 1)
            {
                if (tail.Row == head.Row - 1 || tail.Row == head.Row || tail.Row == head.Row + 1)
                    return tail;
            }

            //is touching top? 
            if (tail.Col == head.Col - 1)
            {
                if (tail.Row == head.Row - 1 || tail.Row == head.Row || tail.Row == head.Row + 1)
                    return tail;
            }

            //is touching left/right? 
            if (tail.Row == head.Row)
            {
                if (tail.Col == head.Col - 1 || tail.Col == head.Col || tail.Col == head.Col + 1)
                    return tail;
                tail.Col += Math.Sign(head.Col - tail.Col);
                return tail;
            }

            if (Math.Abs(head.Row - tail.Row) == 2 || Math.Abs(head.Col - tail.Col) == 2)
            {
                tail.Col += Math.Sign(head.Col - tail.Col);
                tail.Row += Math.Sign(head.Row - tail.Row);
            }

            return tail;
        }

        private static Position MoveHead(Position head, string direction)
        {
            if (direction == "L") head.Col -= 1;
            if (direction == "R") head.Col += 1;
            if (direction == "U") head.Row -= 1;
            if (direction == "D") head.Row += 1;
            return head;
        }

        private static void DrawSurface(char[][] surface)
        {
            foreach (var line in surface)
            {
                Console.WriteLine(string.Join("", line));
            }
        }
    }

    public class Position
    {
        public char C { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
    }
}
