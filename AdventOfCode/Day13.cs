using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    class Day13
    {
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines(@"Datasets\day13.txt");
            var rows = lines.Length;
            var pair = 0;
            var count = 0;
            var test = new PackageTest();

            for (int r = 0; r < lines.Length; r = r + 3)
            {
                ++pair;
                Console.WriteLine($"== Pair {pair} ==");
                string l1 = lines[r];
                string l2 = lines[r + 1];

                var package1 = ParseData(l1);
                var package2 = ParseData(l2);

                if (test.Compare(package1, package2)) count += pair;
                Console.WriteLine();
            }
            Console.WriteLine($"Sum {count}");

            //Part #2 Sorting 
            var packages = new List<ListData>();

            for (int r = 0; r < lines.Length; r = r + 3)
            {
                string l1 = lines[r];
                string l2 = lines[r + 1];

                var package1 = ParseData(l1);
                var package2 = ParseData(l2);
                packages.Add(package1);
                packages.Add(package2);
            }
            var divider1 = ParseData("[[2]]");
            var divider2 = ParseData("[[6]]");
            packages.Add(divider1);
            packages.Add(divider2);

            for (int i = 0; i < packages.Count - 1; i++)
            {
                for (int j = 0; j < packages.Count - i - 1; j++)
                {
                    if (test.Compare(packages[j], packages[j + 1]))
                    {
                        var tmp = packages[j];
                        packages[j] = packages[j + 1];
                        packages[j + 1] = tmp;
                    }
                }
            }
            packages.Reverse();
            Console.WriteLine($"Key {(packages.IndexOf(divider1) + 1) * (packages.IndexOf(divider2) + 1)}");
        }

        private static List<string> SplitIntegerList(string data)
        {
            return data.Split(",").ToList();
        }

        public static bool IsDataOnlyIntegers(string data)
        {
            return !data.Contains('[') && !data.Contains(']');
        }

        public static List<string> SplitList(string data)
        {
            var item = "";
            var closingCount = 0;
            var openingCount = 0;
            int i = 0;
            var items = new List<string>();

            if (data == "") return items;

            if (IsDataOnlyIntegers(data))
            {
                return SplitIntegerList(data);
            }

            while (i < data.Length)
            {
                if (data[i] == '[')
                {
                    openingCount++;
                }

                if (data[i] == ']')
                {
                    closingCount++;

                    if (openingCount == closingCount)
                    {
                        //Found list item 
                        item += data[i];
                        item = item.Substring(1, item.Length - 2);
                        items.Add(item.ToString());
                        item = "";
                        i++;
                        continue;
                    }
                }

                if (openingCount == closingCount && data[i] == ',')
                {
                    if (item != "")
                    {
                        items.Add(item);
                        item = "";
                    }
                    i++;
                    continue;
                }

                item += data[i];
                i++;
            }
            if (item != "") items.Add(item.ToString());
            return items;
        }

        public static ListData ParseData(string data)
        {
            var items = new List<IDataItem>();
            var dataSplit = SplitList(data);
            if (dataSplit.Count == 1) return new ListData(dataSplit.Single());

            foreach (var item in dataSplit)
            {
                Console.WriteLine($"List item: {item}");
                items.Add(new ListData(item));
            }

            return new ListData(items);

        }

        public enum ComparisonResult
        {
            LowerThan,
            Equal,
            GreaterThan
        }

        public interface IDataItem
        {
            public ComparisonResult Compare(IDataItem right);
        }

        public class SingleItem : IDataItem
        {

            public int Item { get; set; }
            string item;
            public SingleItem(string i)
            {
                item = i;
                Item = int.Parse(item);
            }

            public ComparisonResult Compare(IDataItem right)
            {
                if (right is SingleItem id)
                {
                    Console.WriteLine($"- Compare {Item} vs {id.Item}");
                    if (Item == id.Item) return ComparisonResult.Equal;
                    if (Item < id.Item) return ComparisonResult.LowerThan;
                    return ComparisonResult.GreaterThan;
                }

                if (right is ListData list)
                {
                    var toList = new ListData(this);
                    return toList.Compare(right);
                }

                return ComparisonResult.Equal;
            }

            public override string ToString()
            {
                return item;
            }
        }

        public class ListData : IDataItem
        {

            string list_;

            public List<IDataItem> Data = new List<IDataItem>();

            public ListData(IDataItem item)
            {
                Data.Add(item);
            }

            public ListData(List<IDataItem> items)
            {
                Data.AddRange(items);
            }

            public ListData(string list)
            {
                list_ = list;
                ResolveData(list);
            }

            private void ResolveData(string list)
            {
                if (int.TryParse(list, out var value))
                {
                    Data.Add(new SingleItem(list));
                    return;
                }

                var items = SplitList(list);
                foreach (var item in items)
                {
                    Data.Add(new ListData(item));
                }
            }

            public ComparisonResult Compare(IDataItem right)
            {
                if (right is SingleItem single)
                {
                    if (Data.Count == 0) return ComparisonResult.LowerThan;
                    return Data.First().Compare(single) == ComparisonResult.LowerThan ? ComparisonResult.LowerThan : ComparisonResult.GreaterThan;
                }

                if (right is ListData rList)
                {
                    Console.WriteLine($"- Compare {this.ToString()} vs {right.ToString()}");

                    for (int i = 0; i < Math.Max(Data.Count, rList.Data.Count); i++)
                    {
                        if (this.Data.Count == i)
                        {
                            Console.WriteLine($"- Left side is smaller, so inputs are in the right order");
                            return ComparisonResult.LowerThan;
                        }
                        if (rList.Data.Count == i)
                        {
                            Console.WriteLine($"- Right side is smaller, so inputs are not the right order");
                            return ComparisonResult.GreaterThan;
                        }

                        var l = this.Data[i];
                        var r = rList.Data[i];

                        var comparisonResult = l.Compare(r);
                        if (comparisonResult == ComparisonResult.Equal) continue;
                        if (comparisonResult == ComparisonResult.LowerThan)
                        {
                            Console.WriteLine($"- Left side is smaller, so inputs are in the right order");
                            return comparisonResult;
                        }
                        if (comparisonResult == ComparisonResult.GreaterThan)
                        {
                            Console.WriteLine($"- Right side is smaller, so inputs are not in the right order");
                            return comparisonResult;
                        }
                    }
                    return ComparisonResult.Equal;
                }

                return ComparisonResult.GreaterThan;
            }

            public override string ToString()
            {
                return $"[{list_}]";
            }
        }

        public class PackageTest
        {
            public bool Compare(ListData left, ListData right)
            {
                Console.WriteLine($"- Compare {left} vs {right}");
                if (left.Data.Count == 0 && right.Data.Count != 0)
                {
                    Console.WriteLine($"- Left side ran out of items, so inputs are in the right order");
                    return true;
                }
                for (int i = 0; i < Math.Max(left.Data.Count, right.Data.Count); i++)
                {
                    if (left.Data.Count == i)
                    {
                        Console.WriteLine($"- Left run out of items, so inputs are in the right order");
                        return true;
                    }
                    if (right.Data.Count == i)
                    {
                        Console.WriteLine($"- Right side ran out of items, so inputs are not in the right order");
                        return false;
                    }
                    var l = left.Data[i];
                    var r = right.Data[i];
                    var comparisonResult = l.Compare(r);
                    if (comparisonResult == ComparisonResult.Equal) continue;
                    if (comparisonResult == ComparisonResult.LowerThan)
                    {
                        Console.WriteLine($"- Left side is smaller, so inputs are in the right order");
                        return true;
                    }
                    if (comparisonResult == ComparisonResult.GreaterThan)
                    {
                        Console.WriteLine($" - Right side is smaller, so inputs are not in the right order");
                        return false;
                    }
                }
                Console.WriteLine($"- Left run out of items, so inputs are in the right order");
                return false;
            }

        }
    }
}