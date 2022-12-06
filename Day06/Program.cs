namespace Day06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var input = File.ReadLines("sample.txt").First();
            var input = File.ReadLines("input.txt").First();

            var startOfPacket = GetStartOfPacketPosition(input);
            Console.WriteLine($"Part 1: {startOfPacket}");

            var startOfMessage = GetStartOfMessagePosition(input, startOfPacket);
            Console.WriteLine($"Part 2: {startOfMessage}");
        }

        public static int GetStartOfPacketPosition(string input)
        {
            for (int i = 3; i < input.Length; i++)
            {
                var distinct = input.Skip(i - 3).Take(4).Distinct().Count();
                if (distinct == 4)
                {
                    return i + 1;
                }
            }

            throw new Exception("No start-of-packet marker found");
        }

        public static int GetStartOfMessagePosition(string input, int startOfPacket)
        {
            for (int i = Math.Max(startOfPacket, 13); i < input.Length; i++)
            {
                var distinct = input.Skip(i - 13).Take(14).Distinct().Count();
                if (distinct == 14)
                {
                    return i + 1;
                }
            }

            throw new Exception("No start-of-message marker found");
        }
    }
}