using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace AdventOfCode
{
    class Day8
    {
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines(@"Datasets\day8.txt");
            var grid = new int[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                var row = line.ToCharArray().Select(x => int.Parse(x.ToString())).ToArray();
                grid[i] = row;
                // Console.WriteLine($"{string.Join("", row)}");
            }

            var g = new Grid(grid);
            var visibility = new int[lines.Length][];
            var viewingDistance = new int[lines.Length][];
            var count = 0;
            for (int c = 0; c < grid.Length; c++)
            {
                visibility[c] = new int[grid[c].Length];
                viewingDistance[c] = new int[grid[c].Length];
                for (int r = 0; r < grid[c].Length; r++)
                {
                    if (r == 0 || r == grid.Length - 1 || c == 0 || c == grid[r].Length - 1) { visibility[c][r] = 1; viewingDistance[c][r] = 0; count++; continue; }

                    var tree = grid[c][r];
                    var isVisibileFromToRight = g.GetRowFromToRight(c, r).All(x => tree > x);
                    var isVisibileFromToLeft = g.GetRowFromToLeft(c, r).All(x => tree > x);
                    var isVisibileFromToTop = g.GetColumnFromToTop(c, r).All(x => tree > x);
                    var isVisibileFromToBottom = g.GetColumnFromToBottom(c, r).All(x => tree > x);
                    var isVisible = isVisibileFromToRight || isVisibileFromToLeft || isVisibileFromToTop || isVisibileFromToBottom;
                    visibility[c][r] = isVisible ? 1 : 0;
                    if (isVisible) count++;

                    var distanceFromToRight = g.GetDistanceForRowFromToRight(c, r, tree);
                    var distanceFromToLeft = g.GetDistanceForRowFromToLeft(c, r, tree);
                    var distanceFromToTop = g.GetDistanceForColumnFromToTop(c, r, tree);
                    var distanceFromToBottom = g.GetDistanceForColumnFromToBottom(c, r, tree);
                    var distance = distanceFromToRight * distanceFromToLeft * distanceFromToTop * distanceFromToBottom;
                    viewingDistance[c][r] = distance;
                }
                // Console.WriteLine($"{string.Join("", viewingDistance[c])}");
            }
            Console.WriteLine($"{count}");
            var max = viewingDistance.Select(x => x.ToList().Max()).Max();
            Console.WriteLine($"{max}");
        }

        public class Tree
        {
            public int Height { get; set; }
            public int RowIx { get; set; }
            public int ColumnIx { get; set; }
            public List<int> Row = new List<int>();
            public List<int> Column = new List<int>();

            public bool IsVisible()
            {
                return Row.Count(x => x > Height) > Row.Count && Column.Count(x => x > Height) > Column.Count ? true : false;
            }
        }

        public class Grid
        {
            int[][] grid_;
            public Grid(int[][] grid)
            {
                grid_ = grid;
            }

            public List<int> GetRowFromToRight(int x, int y)
            {
                var row = new List<int>();

                for (int r = y + 1; r < grid_[x].Length; r++)
                {
                    row.Add(grid_[x][r]);
                }
                return row;
            }

            public int GetDistanceForRowFromToRight(int c, int y, int h)
            {
                var score = 0;
                for (int r = y + 1; r < grid_[y].Length; r++)
                {
                    if (grid_[c][r] < h) score++;
                    if (grid_[c][r] >= h) { score++; break; };
                }
                //Console.WriteLine($"{h}=>{string.Join("", GetRowFromToRight(c, y))} : {score}");
                return score;
            }


            public int GetDistanceForRowFromToLeft(int c, int y, int h)
            {
                var score = 0;
                for (int r = y - 1; 0 <= r; r--)
                {
                    if (grid_[c][r] < h) score++;
                    if (grid_[c][r] >= h) { score++; break; };
                }
                //Console.WriteLine($"{string.Join("", GetRowFromToLeft(c, y))}<={h} : {score}");
                return score;
            }

            public List<int> GetRowFromToLeft(int x, int y)
            {
                var row = new List<int>();
                for (int r = y - 1; 0 <= r; r--)
                {
                    row.Add(grid_[x][r]);
                }
                return row;
            }

            public List<int> GetColumnFromToTop(int x, int r)
            {
                var col = new List<int>();

                for (int c = x - 1; 0 <= c; c--)
                {
                    col.Add(grid_[c][r]);
                }

                return col;
            }

            public List<int> GetColumnFromToBottom(int x, int r)
            {
                var col = new List<int>();

                for (int c = x + 1; c < grid_[x].Length; c++)
                {
                    col.Add(grid_[c][r]);
                }

                return col;
            }

            public int GetDistanceForColumnFromToBottom(int x, int r, int h)
            {
                int score = 0;
                for (int c = x + 1; c < grid_[x].Length; c++)
                {
                    if (grid_[c][r] < h) score++;
                    if (grid_[c][r] >= h) { score++; break; };
                }
                //Console.WriteLine($"{string.Join("", GetColumnFromToTop(x, r))}V{h} : {score}");
                return score;
            }

            public int GetDistanceForColumnFromToTop(int x, int r, int h)
            {
                int score = 0;
                for (int c = x - 1; 0 <= c; c--)
                {
                    if (grid_[c][r] < h) score++;
                    if (grid_[c][r] >= h) { score++; break; };
                }
                //Console.WriteLine($"{string.Join("", GetColumnFromToBottom(x, r))}^{h} : {score}");
                return score;
            }

        }
    }
}
