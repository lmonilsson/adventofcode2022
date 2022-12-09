using System.ComponentModel;

namespace Day09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var steps = File.ReadLines("sample.txt")
            //var steps = File.ReadLines("sample2.txt")
            var steps = File.ReadLines("input.txt")
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => (Delta: MakeDelta(x[0]), Steps: int.Parse(x[2..])))
                .ToList();

            var knotsPart1 = Enumerable.Range(0, 2).Select(_ => (X: 0, Y: 0)).ToList();
            var visitedPart1 = new HashSet<(int, int)>();
            visitedPart1.Add((0, 0));

            foreach (var step in steps)
            {
                Move(knotsPart1, step, visitedPart1);
            }

            Console.WriteLine($"Part 1: {visitedPart1.Count}");

            var knotsPart2 = Enumerable.Range(0, 10).Select(_ => (X: 0, Y: 0)).ToList();
            var visitedPart2 = new HashSet<(int, int)>();
            visitedPart2.Add((0, 0));

            foreach (var step in steps)
            {
                Move(knotsPart2, step, visitedPart2);
            }

            Console.WriteLine($"Part 2: {visitedPart2.Count}");
        }

        private static void Move(List<(int X, int Y)> knots, ((int X, int Y) Delta, int Steps) step, HashSet<(int, int)> tailVisited)
        {
            for (int i = 0; i < step.Steps; i++)
            {
                var (hx, hy) = knots[0];
                hx += step.Delta.X;
                hy += step.Delta.Y;
                knots[0] = (hx, hy);

                for (int k = 1; k < knots.Count; k++)
                {
                    var (kx, ky) = knots[k];
                    if (k < knots.Count - 1)
                    {
                        (kx, ky) = Follow(knots[k - 1].X, knots[k - 1].Y, kx, ky);
                    }
                    else
                    {
                        (kx, ky) = Follow(knots[k - 1].X, knots[k - 1].Y, kx, ky, tailVisited);
                    }

                    knots[k] = (kx, ky);
                }
            }
        }

        private static (int X, int Y) Follow(int hx, int hy, int tx, int ty, HashSet<(int, int)>? visited = null)
        {
            while (Math.Abs(hx - tx) + Math.Abs(hy - ty) > 1 && !(Math.Abs(hx - tx) == 1 && Math.Abs(hy - ty) == 1))
            {
                if (hx > tx && hy > ty)
                {
                    tx += 1;
                    ty += 1;
                }
                else if (hx > tx && hy < ty)
                {
                    tx += 1;
                    ty -= 1;
                }
                else if (hx < tx && hy > ty)
                {
                    tx -= 1;
                    ty += 1;
                }
                else if (hx < tx && hy < ty)
                {
                    tx -= 1;
                    ty -= 1;
                }
                else if (hx > tx)
                {
                    tx += 1;
                }
                else if (hx < tx)
                {
                    tx -= 1;
                }
                else if (hy > ty)
                {
                    ty += 1;
                }
                else if (hy < ty)
                {
                    ty -= 1;
                }

                visited?.Add((tx, ty));
            }

            return (tx, ty);
        }

        private static (int X, int Y) MakeDelta(char dir)
        {
            switch (dir)
            {
                case 'U':
                    return (0, -1);
                case 'R':
                    return (1, 0);
                case 'D':
                    return (0, 1);
                case 'L':
                    return (-1, 0);
                default:
                    throw new Exception($"Invalid direction {dir}");
            }
        }
    }
}