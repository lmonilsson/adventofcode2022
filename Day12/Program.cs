namespace Day12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var grid = File.ReadLines("sample.txt")
            var grid = File.ReadLines("input.txt")
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => x.ToList())
                .ToList();

            var start = FindPos(grid, 'S');
            var end = FindPos(grid, 'E');

            grid[start.Y][start.X] = 'a';
            grid[end.Y][end.X] = 'z';

            var distance = FindShortestDistance(grid, start, end);
            Console.WriteLine($"Part 1: {distance}");

            var bestDistanceFromLowPoint = int.MaxValue;
            for (int y = 0; y < grid.Count; y++)
            {
                for (int x = 0; x < grid[0].Count; x++)
                {
                    if (grid[y][x] == 'a')
                    {
                        var thisDist = FindShortestDistance(grid, (x, y), end);
                        if (thisDist.HasValue && thisDist < bestDistanceFromLowPoint)
                        {
                            bestDistanceFromLowPoint = thisDist.Value;
                        }
                    }
                }
            }

            Console.WriteLine($"Part 2: {bestDistanceFromLowPoint}");
        }

        private static int? FindShortestDistance(List<List<char>> grid, (int X, int Y) start, (int X, int Y) end)
        {
            var queue = new PriorityQueue<(int X, int Y, int Steps), int>();
            var shortestUsed = new Dictionary<(int X, int Y), int>();

            queue.Enqueue((start.X, start.Y, 0), 0);
            shortestUsed[start] = 0;

            while (queue.Count > 0)
            {
                var step = queue.Dequeue();
                var stepPos = (step.X, step.Y);
                if (stepPos == end)
                {
                    return step.Steps;
                }

                foreach (var next in NextValidSteps(grid, stepPos))
                {
                    var nextSteps = step.Steps + 1;
                    if (!shortestUsed.TryGetValue(next, out var nextShortest) || nextSteps < nextShortest)
                    {
                        var nextPriority = nextSteps + ManhattanDistance(next, end);
                        queue.Enqueue((next.X, next.Y, nextSteps), nextPriority);
                        shortestUsed[next] = nextSteps;
                    }
                }
            }

            return null;
        }

        private static int ManhattanDistance((int X, int Y) cur, (int X, int Y) end)
        {
            return Math.Abs(cur.X - end.X) + Math.Abs(cur.Y - end.Y);
        }

        private static IEnumerable<(int X, int Y)> NextValidSteps(List<List<char>> grid, (int X, int Y) pos)
        {
            var candidates = new List<(int X, int Y)>();
            if (pos.X > 0 && CanStep(grid, pos, (pos.X - 1, pos.Y)))
            {
                candidates.Add((pos.X - 1, pos.Y));
            }
            if (pos.X < grid[0].Count - 1 && CanStep(grid, pos, (pos.X + 1, pos.Y)))
            {
                candidates.Add((pos.X + 1, pos.Y));
            }
            if (pos.Y > 0 && CanStep(grid, pos, (pos.X, pos.Y - 1)))
            {
                candidates.Add((pos.X, pos.Y - 1));
            }
            if (pos.Y < grid.Count - 1 && CanStep(grid, pos, (pos.X, pos.Y + 1)))
            {
                candidates.Add((pos.X, pos.Y + 1));
            }
            return candidates;
        }

        private static bool CanStep(List<List<char>> grid, (int X, int Y) pos, (int X, int Y) next)
        {
            return grid[next.Y][next.X] <= grid[pos.Y][pos.X] + 1;
        }

        private static (int X, int Y) FindPos(List<List<char>> grid, char needle)
        {
            var pos = grid
                .SelectMany((row, y) => row.Select((c, x) => (c, x, y)))
                .First(cxy => cxy.c == needle);

            return (pos.x, pos.y);
        }
    }
}
