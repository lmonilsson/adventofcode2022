using System.Text;

namespace Day10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var instructions = File.ReadLines("sample.txt")
            var instructions = File.ReadLines("input.txt")
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => (Instr: x[..4], Param: x.Length == 4 ? 0 : int.Parse(x[5..])))
                .ToList();

            var regx = 1;
            var pc = 0;
            var cycle = 0;
            var addxCycle = -1;
            var signalStrength = 0;
            var output = new StringBuilder();

            while (pc < instructions.Count)
            {
                cycle++;

                if (cycle == 20 || cycle == 60 || cycle == 100 || cycle == 140 || cycle == 180 || cycle == 220)
                {
                    signalStrength += regx * cycle;
                }

                var pixel = (cycle - 1) % 40;
                if (pixel == 0 && output.Length > 0)
                {
                    output.AppendLine();
                }
                output.Append(pixel >= regx - 1 && pixel <= regx + 1 ? '#' : '.');

                if (addxCycle >= 1 && cycle == addxCycle + 1)
                {
                    regx += instructions[pc].Param;
                    addxCycle = -1;
                    pc++;
                }
                else if (addxCycle == -1 && instructions[pc].Instr == "addx")
                {
                    addxCycle = cycle;
                }
                else if (addxCycle == -1)
                {
                    pc++;
                }
            }

            Console.WriteLine($"Part 1: {signalStrength}");

            Console.WriteLine("Part 2:");
            Console.WriteLine(output.ToString());
        }
    }
}