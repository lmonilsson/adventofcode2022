using System;
using System.IO;
using System.Linq;

namespace Day03
{
    class Program
    {
        static void Main(string[] args)
        {
            //var rucksacks = File.ReadAllLines("sample.txt")
            var rucksacks = File.ReadAllLines("input.txt")
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            var totalPriority = 0;
            foreach (var r in rucksacks)
            {
                var shared = r
                    .Skip(r.Length / 2)
                    .First(c => r.Take(r.Length / 2).Contains(c));

                if (char.IsLower(shared))
                {
                    totalPriority += shared - 'a' + 1;
                }
                else
                {
                    totalPriority += shared - 'A' + 27;
                }
            }

            Console.WriteLine($"Part 1: {totalPriority}");

            var totalBadgePriority = 0;
            for (int i = 0; i < rucksacks.Count; i += 3)
            {
                var badge = rucksacks[i]
                    .Intersect(rucksacks[i + 1])
                    .Intersect(rucksacks[i + 2])
                    .Single();

                if (char.IsLower(badge))
                {
                    totalBadgePriority += badge - 'a' + 1;
                }
                else
                {
                    totalBadgePriority += badge - 'A' + 27;
                }
            }

            Console.WriteLine($"Part 2: {totalBadgePriority}");
        }
    }
}
