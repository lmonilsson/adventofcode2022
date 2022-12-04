using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    class Program
    {
        static void Main(string[] args)
        {
            var elves = new List<int>();
            elves.Add(0);
            foreach (var line in File.ReadAllLines("input.txt"))
            {
                if (string.IsNullOrEmpty(line))
                {
                    elves.Add(0);
                }
                else
                {
                    elves[elves.Count - 1] += int.Parse(line);
                }
            }

            int max = elves.Max();
            Console.WriteLine($"Part 1: {max}");

            var top3 = elves.OrderByDescending(x => x).Take(3).Sum();
            Console.WriteLine($"Part 2: {top3}");
        }
    }
}
