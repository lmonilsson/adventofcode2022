using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            var choiceScores = new Dictionary<char, int>
            {
                { 'X', 1 },
                { 'Y', 2 },
                { 'Z', 3 },
            };

            var vsScores = new Dictionary<(char Theirs, char Mine), int>
            {
                { ('A', 'X'), 3 },
                { ('A', 'Y'), 6 },
                { ('A', 'Z'), 0 },
                { ('B', 'X'), 0 },
                { ('B', 'Y'), 3 },
                { ('B', 'Z'), 6 },
                { ('C', 'X'), 6 },
                { ('C', 'Y'), 0 },
                { ('C', 'Z'), 3 },
            };

            var actualChoices = new Dictionary<(char, char), char>
            {
                { ('A', 'X'), 'Z' },
                { ('A', 'Y'), 'X' },
                { ('A', 'Z'), 'Y' },
                { ('B', 'X'), 'X' },
                { ('B', 'Y'), 'Y' },
                { ('B', 'Z'), 'Z' },
                { ('C', 'X'), 'Y' },
                { ('C', 'Y'), 'Z' },
                { ('C', 'Z'), 'X' },
            };

//            var lines = @"
//A Y
//B X
//C Z".Split(Environment.NewLine);

            var lines = File.ReadLines("input.txt");

            var part1Score = lines
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => choiceScores[x[2]] + vsScores[(x[0], x[2])])
                .Sum();

            Console.WriteLine($"Part 1: {part1Score}");

            var part2Score = lines
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => (Their: x[0], Mine: actualChoices[(x[0], x[2])]))
                .Select(x => choiceScores[x.Mine] + vsScores[(x.Their, x.Mine)])
                .Sum();

            Console.WriteLine($"Part 2: {part2Score}");
        }
    }
}
