namespace Day13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("sample.txt")
            //var lines = File.ReadLines("sample.txt")
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            var first = Value.Parse(lines[3]);
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
                //while (consumed != input.Length/* && input[consumed] != ']'*/)
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
                // Consumed the number.
                var number = int.Parse(input[..numberEnd]);
                return (Value: new Value(number), Consumed: numberEnd);
                //if (numberEnd == 0)
                //{
                //    // Found ']' on the first position, so it is an empty list.
                //    return (Value: new Value(new Value[0]), Consumed: 1);
                //}
                //else
                //{
                //}
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
    }
}
