using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace AdventOfCode
{
    class Day7
    {
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines(@"Datasets\day7.txt");
            var paths = new List<string>();
            var current = "";
            var files = new List<File>();
            foreach (var line in lines)
            {
                var a = line.Split(" ");
                Console.WriteLine(@$"Command: {line}");

                if (a[0] == "dir")
                {
                    //Contains directory 
                    var dir = current + "/" + a[1];
                    if (!paths.Contains(dir)) paths.Add(dir);
                    Console.WriteLine(@$"Current Path: {current}");
                    continue;
                }

                if (int.TryParse(a[0], out var size))
                {
                    //Store paths and files with paths
                    var fileName = a[1];
                    var path = current + $"/{a[1]}+{a[0]}";
                    if (!paths.Contains(current)) paths.Add(current);
                    files.Add(new File(current, fileName, size));
                    Console.WriteLine(@$"File Path: {path}");
                    continue;
                }

                if (a[0] == "$" && a[1] == "cd" && a[2] == "..")
                {
                    //Move out one level
                    var ds = current.Split("/").ToList();
                    current = string.Join("/", ds.Take(ds.Count - 1));
                    Console.WriteLine(@$"Current Path: {current}");
                    continue;
                }

                if (a[0] == "$" && a[1] == "cd" && a[2] == "/")
                {
                    //Move to top
                    current = "";
                    Console.WriteLine(@$"Current Path: {current}");
                    continue;
                }

                if (a[0] == "$" && a[1] == "cd" && a[2] != "/"! & a[2] != "..")
                {
                    //Move to in the directory
                    current = current + "/" + a[2];
                    Console.WriteLine(@$"Current Path: {current}");
                    continue;
                }

                if (a[0] == "$" && a[1] == "ls")
                {
                    //List
                    Console.WriteLine(@$"Current Path: - {current}");
                    continue;
                }
            }

            Console.WriteLine();

            var totalSum = 0;
            var usedSpace = files.Where(x => x.Path.Contains("")).Sum(x => x.Size);
            var unusedSpace = 70000000 - usedSpace;
            var sizes = new List<int>();

            foreach (var path in paths.Distinct())
            {
                var size = files.Where(x => x.Path.Contains(path)).Sum(x => x.Size);
                sizes.Add(size);
                if (size <= 100000) totalSum += size;
                Console.WriteLine(@$"Path: {path} size {size}");
            }
            
            var toDelete = sizes.Select(x => x).Where(x => x > usedSpace - 40000000).Min();
            Console.WriteLine(@$"TotalSum: {totalSum}");
            Console.WriteLine(@$"UnusedSpace: {unusedSpace}");
            Console.WriteLine(@$"UsedSpace  : {usedSpace}");
            Console.WriteLine(@$"ToDelete   : {toDelete}");
        }

        public class File
        {
            public File(string path, string name, int size)
            {
                Path = path;
                Name = name;
                Size = size;
            }
            public string Path { get; set; }
            public string Name { get; set; }
            public int Size { get; set; }
        }

    }
}
