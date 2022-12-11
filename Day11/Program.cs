using System.Data.Common;
using System.Text.RegularExpressions;

namespace Day11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var input = File.ReadLines("sample.txt")
            var input = File.ReadLines("input.txt")
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            var monkeys = Parse(input);

            for (int round = 0; round < 20; round++)
            {
                foreach (var monkey in monkeys)
                {
                    var toMove = new List<(Item Item, int MonkeyIdx)>();
                    for (int itemIdx = 0; itemIdx < monkey.Items.Count; itemIdx++)
                    {
                        monkey.NumInspections++;

                        var item = monkey.Items[itemIdx];
                        item.WorryLevel = monkey.Operation(item.WorryLevel) / 3;
                        if (item.WorryLevel % monkey.TestDivisibleBy == 0)
                        {
                            toMove.Add((item, monkey.ThrowToIfTrue));
                        }
                        else
                        {
                            toMove.Add((item, monkey.ThrowToIfFalse));
                        }
                    }

                    foreach (var move in toMove)
                    {
                        monkey.Items.Remove(move.Item);
                        monkeys[move.MonkeyIdx].Items.Add(move.Item);
                    }
                }
            }

            var monkeyBusiness = monkeys.OrderByDescending(m => m.NumInspections).Take(2).Aggregate(1, (agg, m) => agg * m.NumInspections);
            Console.WriteLine($"Part 1: {monkeyBusiness}");
        }

        private static List<Monkey> Parse(List<string> input)
        {
            var monkeys = new List<Monkey>();

            for (int i = 0; i < input.Count; i += 6)
            {
                if (!input[i].StartsWith("Monkey"))
                {
                    throw new Exception("Parse error: Expected line to start with \"Monkey\"");
                }

                var items = input[i + 1].Split(':')[1].Split(", ")
                    .Select(x => new Item { WorryLevel = int.Parse(x) })
                    .ToList();

                var opMatch = Regex.Match(input[i + 2], "new = old ([^ ]+) (.+)");
                var op = opMatch.Groups[1].Value;
                var operand = opMatch.Groups[2].Value;

                Func<int, int> operation;
                if (operand == "old")
                {
                    operation = (int old) => old * old;
                }
                else
                {
                    var operandNum = int.Parse(operand);
                    switch (op)
                    {
                        case "*":
                            operation = (int old) => old * operandNum;
                            break;
                        case "+":
                            operation = (int old) => old + operandNum;
                            break;
                        default:
                            throw new Exception($"Parse error: Unexpected operator {op}");
                    }
                }

                var divisibleBy = int.Parse(input[i + 3].Split()[^1]);
                var throwToIfTrue = int.Parse(input[i + 4].Split()[^1]);
                var throwToIfFalse = int.Parse(input[i + 5].Split()[^1]);

                monkeys.Add(new Monkey
                {
                    Items = items,
                    Operation = operation,
                    TestDivisibleBy = divisibleBy,
                    ThrowToIfTrue = throwToIfTrue,
                    ThrowToIfFalse = throwToIfFalse
                });
            }

            return monkeys;
        }
    }

    internal class Monkey
    {
        public List<Item> Items { get; set; }
        public Func<int, int> Operation { get; set; }
        public int TestDivisibleBy { get; set; }
        public int ThrowToIfTrue { get; set; }
        public int ThrowToIfFalse { get; set; }
        public int NumInspections { get; set; }
    }

    internal class Item
    {
        public int WorryLevel { get; set; }
    }
}
