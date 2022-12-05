namespace Day05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var lines = File.ReadAllLines("sample.txt");
            var lines = File.ReadAllLines("input.txt");

            var part1Stacks = RunSimulation(lines, CraneModel.CrateMover9000);
            var part1Top = new string(part1Stacks.Where(x => x.Any()).Select(x => x.Last()).ToArray());
            Console.WriteLine($"Part 1: {part1Top}");

            var part2Stacks = RunSimulation(lines, CraneModel.CrateMover9001);
            var part2Top = new string(part2Stacks.Where(x => x.Any()).Select(x => x.Last()).ToArray());
            Console.WriteLine($"Part 2: {part2Top}");
        }

        private static List<List<char>> RunSimulation(IEnumerable<string> lines, CraneModel model)
        {
            var phase = Phase.InitialStacks;
            var stacks = new List<List<char>>();

            foreach (var line in lines)
            {
                if (line == string.Empty)
                {
                    continue;
                }

                switch (phase)
                {
                    case Phase.InitialStacks:
                        if (line[0] == '[' || line[1] == ' ')
                        {
                            // "[N] [C]    "
                            for (int i = 0; i < line.Length; i += 4)
                            {
                                if (stacks.Count <= i / 4)
                                {
                                    stacks.Add(new List<char>());
                                }

                                if (line[i] == '[')
                                {
                                    stacks[i / 4].Add(line[i + 1]);
                                }
                            }
                        }
                        else
                        {
                            // " 1   2   3 "
                            var maxNumber = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => int.Parse(x))
                                .Last();

                            if (maxNumber != stacks.Count)
                            {
                                throw new Exception($"Parse error: wrong number of stacks: expected {maxNumber}, was {stacks.Count})");
                            }

                            foreach (var stack in stacks)
                            {
                                stack.Reverse();
                            }

                            phase = Phase.Instructions;
                        }

                        break;

                    case Phase.Instructions:
                        // "move 1 from 2 to 1"
                        var parts = line.Split();
                        var num = int.Parse(parts[1]);
                        var from = int.Parse(parts[3]);
                        var to = int.Parse(parts[5]);

                        var fromStack = stacks[from - 1];
                        var toStack = stacks[to - 1];

                        switch (model)
                        {
                            case CraneModel.CrateMover9000:
                                for (int i = 0; i < num; i++)
                                {
                                    var crate = fromStack.Last();
                                    fromStack.RemoveAt(fromStack.Count - 1);
                                    toStack.Add(crate);
                                }

                                break;
                            case CraneModel.CrateMover9001:
                                toStack.AddRange(fromStack.Skip(fromStack.Count - num).Take(num));
                                fromStack.RemoveRange(fromStack.Count - num, num);

                                break;

                            default:
                                throw new Exception($"Unsupported crane model {model}");
                        }

                        break;

                    default:
                        throw new Exception($"Invalid phase {phase}");
                }
            }

            return stacks;
        }

        enum Phase
        {
            InitialStacks,
            Instructions
        }

        enum CraneModel
        {
            CrateMover9000,
            CrateMover9001
        }
    }
}
