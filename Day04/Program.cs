namespace Day04
{
    class Program
    {
        static void Main(string[] args)
        {
            //var pairs = File.ReadLines("sample.txt")
            var pairs = File.ReadLines("input.txt")
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => ParsePair(x))
                .ToList();

            var numFullyContaining = pairs.Count(p => p.First.Swallows(p.Second) || p.Second.Swallows(p.First));
            Console.WriteLine($"Part 1: {numFullyContaining}");

            var numOverlapping = pairs.Count(p => p.First.Overlaps(p.Second));
            Console.WriteLine($"Part 2: {numOverlapping}");
        }

        private static (Interval First, Interval Second) ParsePair(string input)
        {
            var span = input.AsSpan();
            var comma = span.IndexOf(',');
            var first = ParseInterval(span[0..comma]);
            var second = ParseInterval(span[(comma + 1)..]);
            return (first, second);
        }

        private static Interval ParseInterval(ReadOnlySpan<char> input)
        {
            var dash = input.IndexOf('-');
            var min = int.Parse(input[0..dash]);
            var max = int.Parse(input[(dash + 1)..]);
            return new Interval(min, max);
        }
    }

    record Interval(int Min, int Max)
    {
        public bool Swallows(Interval other)
        {
            return Min <= other.Min && Max >= other.Max;
        }

        public bool Overlaps(Interval other)
        {
            return Min <= other.Min && Max >= other.Min
                || Min <= other.Max && Max >= other.Max
                || other.Min <= Min && other.Max >= Min
                || other.Min <= Max && other.Max >= Max;
        }
    }
}
