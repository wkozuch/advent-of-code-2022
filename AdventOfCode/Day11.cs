using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    class Day11
    {
        static void Main(string[] args)
        {
            var monekys = new List<Monkey>() {
                // new Monkey( new []{ 79, 98 }, ( x=> x * 19 ), 23, 2, 3),
                // new Monkey( new []{ 54, 65, 75, 74}, ( x=> x + 6 ), 19, 2, 0),
                // new Monkey( new []{ 79, 60, 97}, ( x=> x * x ), 13, 1, 3),
                // new Monkey( new []{ 74 }, ( x=> x + 3 ), 17, 0, 1),
                new Monkey( new []{ 57 }, ( x=> x *13 ),11, 3, 2),
                new Monkey( new []{ 58, 93, 88, 81, 72, 73, 65}, ( x=> x + 2 ),7, 6, 7),
                new Monkey( new []{ 65, 95 }, ( x=> x + 6 ),13, 3, 5),
                new Monkey( new []{ 58, 80, 81, 83 }, ( x=> x *x ),5, 4, 5),
                new Monkey( new []{ 58, 89, 90, 96, 55 }, ( x=> x + 3 ), 3, 1, 7),
                new Monkey( new []{ 66, 73, 87, 58, 62, 67 }, ( x => x *7), 17, 4, 1),
                new Monkey( new []{ 85, 55, 89 }, ( x => x +4), 2, 2, 0),
                new Monkey( new []{ 73, 80, 54, 94, 90, 52, 69, 58 }, ( x => x + 7), 19, 6, 0)
            };
            var monkeysList = monekys;
            var lcd = monkeysList.Select( x => x.Divisor).Aggregate( (d, x) => d * x);

            for (int round = 1; round <= 10000; round++)
            {
                Console.WriteLine($"Round #{round}");
                var m = 0;
                
                for (int i = 0; i < monkeysList.Count; i++)
                {
                    Monkey monkey = monkeysList[i];
                    //Console.WriteLine($"Monkey {i}");
                    var items = monkey.Items;
                    foreach (var item in items.ToList())
                    {
                        var result = monkey.InspectAndThrowTo(item, lcd);

                        monkeysList[result.Item2].Items.Add(result.Item1);
                    }
                }

                monkeysList = monekys;

                foreach (var mon in monekys)
                {
                    Console.WriteLine($"Monkey {m++} inspected items {mon.Inspects} times.");
                }
            }
            var inspects = monkeysList.Select(x => x.Inspects).OrderByDescending(x => x).ToList();
            var monekeyBusiness = inspects.First() * inspects.Skip(1).First();
            Console.WriteLine(monekeyBusiness);
        }

        public class Monkey
        {
            public double Inspects = 0;
            private readonly int throwToMonkeySuccess;
            private readonly int throwToMonkeyFail;
            public int Divisor;

            public Monkey(int[] startingItems,
            Func<long, long> operation,
            int testDividor,
                          int throwToMonkeySuccess,
                          int throwToMonkeyFail)
            {
                Items = startingItems.Select(x => (long)x).ToList();
                Operation = operation;
                this.Divisor = testDividor;
                this.throwToMonkeySuccess = throwToMonkeySuccess;
                this.throwToMonkeyFail = throwToMonkeyFail;
            }

            public List<long> Items { get; }

            public Func<long, long> Operation;

            public bool Test(long worryLevel)
            {
                return worryLevel % Divisor == 0;
            }

            public (long, int) InspectAndThrowTo(long item, long lcd)
            {
                var log = false;
                Inspects++;
                
                if (log) Console.WriteLine($"Monkey inspects an item with a worry level of {item}.");
                Items.Remove((int)item);

                var worryLevel = Operation(item);
                if (log) Console.WriteLine($"Worry level {item} is changed by operation to {worryLevel}.");

                var newLevel = worryLevel % lcd;
                if (log) Console.WriteLine($"Monkey gets bored with item. Worry level is divided by 3 to {newLevel}.");

                var isDivisible = Test(newLevel) ? "is" : "is not";
                if (log) Console.WriteLine($"Current worry level {isDivisible} divisible by {Divisor}.");

                var result = Test(newLevel) ? (newLevel, throwToMonkeySuccess) : (newLevel, throwToMonkeyFail);
                if (log) Console.WriteLine($"Item {item} with worry level {result.Item1} is thrown to monkey {result.Item2}.");
                return result;
            }
        }
    }
}