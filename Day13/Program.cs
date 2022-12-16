using System.Text.RegularExpressions;

namespace Day13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var packets = File.ReadLines("sample.txt")
            var packets = File.ReadLines("input.txt")
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => Value.Parse(x))
                .ToList();

            var invalidPairIndices = new List<int>();
            for (int i = 0; i < packets.Count; i += 2)
            {
                var first = packets[i];
                var second = packets[i + 1];

                if (first.CompareTo(second) <= 0)
                {
                    invalidPairIndices.Add(i / 2 + 1);
                }
            }

            var invalidSum = invalidPairIndices.Sum();
            Console.WriteLine($"Part 1: {invalidSum}");

            var orderedPackets = packets.ToList();
            orderedPackets.Sort((a, b) => a.CompareTo(b));

            var dividerPacket1 = Value.Parse("[[2]]");
            var dividerPacket2 = Value.Parse("[[6]]");
            var dividerPacketsToInsert = new List<Value>
            {
                dividerPacket1,
                dividerPacket2
            };

            for (int i = 0; i <= packets.Count && dividerPacketsToInsert.Any(); i++)
            {
                if ((i < packets.Count && orderedPackets[i].CompareTo(dividerPacketsToInsert[0]) > 0)
                    || i == packets.Count)
                {
                    orderedPackets.Insert(i, dividerPacketsToInsert[0]);
                    dividerPacketsToInsert.RemoveAt(0);
                    i++;
                }
            }

            var firstDividerIndex = orderedPackets.IndexOf(dividerPacket1) + 1;
            var secondDividerIndex = orderedPackets.IndexOf(dividerPacket2) + 1;
            var decoderKey = firstDividerIndex * secondDividerIndex;
            Console.WriteLine($"Part 2: {decoderKey}");
        }
    }

    internal record Value(IReadOnlyList<Value>? Values, int? Number)
    {
        public Value(IReadOnlyList<Value> values): this(values, null) {}
        public Value(int number) : this(null, number) {}

        public static Value Parse(ReadOnlySpan<char> input)
        {
            if (input.Length == 0 || input[0] != '[')
            {
                throw new Exception("Value must start with '['");
            }

            return ParseInternal(input).Value;
        }

        private static (Value Value, int Consumed) ParseInternal(ReadOnlySpan<char> input)
        {
            // [[1],[2,3,[],4]]
            if (input[0] == '[')
            {
                var values = new List<Value>();
                var consumed = 1;
                while (input[consumed] != ']')
                {
                    var (value, valueConsumed) = ParseInternal(input[consumed..]);
                    values.Add(value);
                    consumed += valueConsumed;

                    if (input[consumed] == ',')
                    {
                        consumed++;
                    }
                }

                // Consume ']'.
                consumed++;

                return (Value: new Value(values), consumed);
            }
            else
            {
                var numberEnd = input.IndexOfAny(new[] { ',', ']' });
                var number = int.Parse(input[..numberEnd]);
                return (Value: new Value(number), Consumed: numberEnd);
            }
        }

        public int CompareTo(Value other)
        {
            if (Number != null && other.Number != null)
            {
                return Number.Value - other.Number.Value;
            }
            else if (Number != null)
            {
                return MakeList(Number.Value).CompareTo(other);
            }
            else if (other.Number != null)
            {
                return CompareTo(MakeList(other.Number.Value));
            }
            else if (Values != null && other.Values != null)
            {
                // Both are lists
                for (int i = 0; i < Math.Max(Values.Count, other.Values.Count); i++)
                {
                    if (Values.Count == i)
                    {
                        // This is smaller.
                        return -1;
                    }
                    else if (other.Values.Count == i)
                    {
                        // This is larger.
                        return 1;
                    }

                    var cmp = Values[i].CompareTo(other.Values[i]);
                    if (cmp != 0)
                    {
                        return cmp;
                    }
                }
            }

            return 0;
        }

        public static Value MakeList(int number)
        {
            return new Value(new[] { new Value(number) });
        }

        public override string ToString()
        {
            if (Values != null)
            {
                return "[" + string.Join(",", Values) + "]";
            }
            else if (Number != null)
            {
                return Number.Value.ToString();
            }

            // Cannot happen.
            return "";
        }
    }
}
