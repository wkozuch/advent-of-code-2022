using System;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    class Day2
    {
        const int Win = 6;
        const int Draw = 3;
        const int Loss = 0;

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(@"Datasets\day2.txt");
            var score = 0;
            foreach (var line in lines)
            {
                var opponentHand = Day2.ToHand(line.Split(" ").First());
                //var responseHand = Day2.ToHand(line.Split(" ").Last());
                var responseHand = Day2.ToExpectedResultHand(opponentHand, line.Split(" ").Last()); // #Task2
                Console.WriteLine($"OpponentHand: {opponentHand} responseHand: {responseHand}: ResultScore: {responseHand.CheckResultAgainst(opponentHand)}");
                score += responseHand.CheckResultAgainst(opponentHand);
            }
            Console.WriteLine($"Score {score}");
        }

        public static IHand ToHand(string c)
        {
            switch(c) {
                case "A": return new Rock();
                case "B": return new Paper();
                case "C": return new Scissors();
                case "X": return new Rock();
                case "Y": return new Paper();
                case "Z": return new Scissors();
                default:
                return null;
            }
        }

        public static IHand ToExpectedResultHand(IHand hand, string expectedResult)
        {
            if (expectedResult == "X") return hand.WinningTo(); //Win
            if (expectedResult == "Y") return hand; //Draw
            return hand.LoosingTo(); //Loss
        }

        public interface IHand
        {
            int Value { get; }
            int CheckResultAgainst(IHand otherHand);
            IHand LoosingTo();
            IHand WinningTo();
        }

        public class Rock : IHand
        {
            public int Value => 1;
            public int CheckResultAgainst(IHand otherHand)
            {
                if (otherHand is Scissors) return Value + Win;
                if (otherHand is Paper) return Value + Loss;
                return Value + Draw;
            }

            public IHand WinningTo() => new Scissors();
            public IHand LoosingTo() => new Paper();
            public override string ToString() => "Rock";
        }

        public class Paper : IHand
        {
            public int Value => 2;
            public int CheckResultAgainst(IHand otherHand)
            {
                if (otherHand is Scissors) return Value + Loss;
                if (otherHand is Rock) return Value + Win;
                return Value + Draw;
            }

            public IHand LoosingTo() => new Scissors();
            public IHand WinningTo() => new Rock();
            public override string ToString() => "Paper";
        }

        public class Scissors : IHand
        {
            public int Value => 3;
            public int CheckResultAgainst(IHand otherHand)
            {
                if (otherHand is Paper) return Value + Win;
                if (otherHand is Rock) return Value + Loss;
                return Value + Draw;
            }
            public IHand WinningTo() => new Paper();
            public IHand LoosingTo() => new Rock();
            public override string ToString() => "Scissors";
        }

    }
}
