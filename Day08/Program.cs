using System.Xml.Xsl;

namespace Day08
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var trees = File.ReadLines("sample.txt")
            var trees = File.ReadLines("input.txt")
                .Where(x => !string.IsNullOrEmpty(x))
                .SelectMany((line, y) => line.Select((h, x) => new Tree(x, y, h)))
                .ToList();

            var visibilityMap = new VisibilityMap(trees);
            var numVisible = trees.Count(t => visibilityMap.IsVisible(t.X, t.Y));
            Console.WriteLine($"Part 1: {numVisible}");

            var scenicMap = new ScenicMap(trees);
            var bestScenicScore = trees.Max(t => scenicMap.ScenicScore(t.X, t.Y));
            Console.WriteLine($"Part 2: {bestScenicScore}");
        }
    }

    internal class Tree
    {
        public int X { get; }
        public int Y { get; }
        public int Height { get; }

        public Tree(int x, int y, int height)
        {
            X = x;
            Y = y;
            Height = height;
        }
    }

    internal class VisibilityMap
    {
        private readonly IReadOnlyDictionary<(int, int), Tree> _trees;
        private int _maxX;
        private int _maxY;
        private Dictionary<(int, int), int> _topHeight = new Dictionary<(int, int), int>();
        private Dictionary<(int, int), int> _rightHeight = new Dictionary<(int, int), int>();
        private Dictionary<(int, int), int> _bottomHeight = new Dictionary<(int, int), int>();
        private Dictionary<(int, int), int> _leftHeight = new Dictionary<(int, int), int>();

        public VisibilityMap(IReadOnlyList<Tree> trees)
        {
            _trees = trees.ToDictionary(t => (t.X, t.Y));
            _maxX = trees.Max(t => t.X);
            _maxY = trees.Max(t => t.Y);
        }

        public bool IsVisible(int x, int y)
        {
            return IsTopVisible(x, y)
                || IsRightVisible(x, y)
                || IsBottomVisible(x, y)
                || IsLeftVisible(x, y);
        }

        private bool IsTopVisible(int x, int y)
        {
            ComputeHeight(x, y, Above, _topHeight);
            var tree = _trees[(x, y)];
            var highest = _topHeight[Above(x, y)];
            return tree.Height > highest;
        }
        private (int, int) Above(int x, int y) => (x, y - 1);

        private bool IsRightVisible(int x, int y)
        {
            ComputeHeight(x, y, Right, _rightHeight);
            var tree = _trees[(x, y)];
            var highest = _rightHeight[Right(x, y)];
            return tree.Height > highest;
        }
        private (int, int) Right(int x, int y) => (x + 1, y);

        private bool IsBottomVisible(int x, int y)
        {
            ComputeHeight(x, y, Bottom, _bottomHeight);
            var tree = _trees[(x, y)];
            var highest = _bottomHeight[Bottom(x, y)];
            return tree.Height > highest;
        }
        private (int, int) Bottom(int x, int y) => (x, y + 1);

        private bool IsLeftVisible(int x, int y)
        {
            ComputeHeight(x, y, Left, _leftHeight);
            var tree = _trees[(x, y)];
            var highest = _leftHeight[Left(x, y)];
            return tree.Height > highest;
        }
        private (int, int) Left(int x, int y) => (x - 1, y);

        private void ComputeHeight(
            int x, int y,
            Func<int, int, (int, int)> next,
            Dictionary<(int, int), int> map)
        {
            if (map.ContainsKey((x, y)))
            {
                return;
            }

            var (nextX, nextY) = next(x, y);

            if (!map.TryGetValue((nextX, nextY), out var nextMaxHeight))
            {
                if (nextX == -1 || nextX == _maxX + 1 || nextY == -1 || nextY == _maxY + 1)
                {
                    map[(nextX, nextY)] = -1;
                    nextMaxHeight = -1;
                }
                else
                {
                    ComputeHeight(nextX, nextY, next, map);
                    nextMaxHeight = map[(nextX, nextY)];
                }
            }

            var tree = _trees[(x, y)];
            map[(x, y)] = Math.Max(tree.Height, nextMaxHeight);
        }
    }

    internal class ScenicMap
    {
        private readonly IReadOnlyDictionary<(int, int), Tree> _trees;
        private int _maxX;
        private int _maxY;
        private Dictionary<(int, int), int> _topViewingDistance = new Dictionary<(int, int), int>();
        private Dictionary<(int, int), int> _rightViewingDistance = new Dictionary<(int, int), int>();
        private Dictionary<(int, int), int> _bottomViewingDistance = new Dictionary<(int, int), int>();
        private Dictionary<(int, int), int> _leftViewingDistance = new Dictionary<(int, int), int>();

        public ScenicMap(IReadOnlyList<Tree> trees)
        {
            _trees = trees.ToDictionary(t => (t.X, t.Y));
            _maxX = trees.Max(t => t.X);
            _maxY = trees.Max(t => t.Y);
        }

        public int ScenicScore(int x, int y)
        {
            return TopViewingDistance(x, y)
                * RightViewingDistance(x, y)
                * BottomViewingDistance(x, y)
                * LeftViewingDistance(x, y);
        }

        private int TopViewingDistance(int x, int y)
        {
            var tree = _trees[(x, y)];
            int distance = 0;
            for (int i = y - 1; i >= 0; i--)
            {
                distance++;
                if (_trees[(x, i)].Height >= tree.Height)
                {
                    break;
                }
            }
            return distance;
        }

        private int RightViewingDistance(int x, int y)
        {
            var tree = _trees[(x, y)];
            int distance = 0;
            for (int i = x + 1; i <= _maxX; i++)
            {
                distance++;
                if (_trees[(i, y)].Height >= tree.Height)
                {
                    break;
                }
            }
            return distance;
        }

        private int BottomViewingDistance(int x, int y)
        {
            var tree = _trees[(x, y)];
            int distance = 0;
            for (int i = y + 1; i <= _maxY; i++)
            {
                distance++;
                if (_trees[(x, i)].Height >= tree.Height)
                {
                    break;
                }
            }
            return distance;
        }

        private int LeftViewingDistance(int x, int y)
        {
            var tree = _trees[(x, y)];
            int distance = 0;
            for (int i = x - 1; i >= 0; i--)
            {
                distance++;
                if (_trees[(i, y)].Height >= tree.Height)
                {
                    break;
                }
            }
            return distance;
        }
    }
}
