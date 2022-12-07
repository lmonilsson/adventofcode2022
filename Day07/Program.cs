namespace Day07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var session = File.ReadLines("sample.txt")
            var session = File.ReadLines("input.txt")
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            var fs = ParseFileSystemFromSession(session);

            var smallDirsTotal = 0;
            TraverseFileSystem(fs, obj =>
            {
                if (obj is FileSystemDirectory d && d.TotalSize <= 100_000)
                {
                    smallDirsTotal += d.TotalSize;
                }
            });

            Console.WriteLine($"Part 1: {smallDirsTotal}");

            var free = 70_000_000 - fs.TotalSize;
            var required = 30_000_000;
            var toFree = required - free;

            var candidateToDelete = fs;
            TraverseFileSystem(fs, obj =>
            {
                if (obj is FileSystemDirectory d && d.TotalSize >= toFree && d.TotalSize < candidateToDelete.TotalSize)
                {
                    candidateToDelete = d;
                }
            });

            Console.WriteLine($"Part 2: {candidateToDelete.TotalSize}");
        }

        private static FileSystemDirectory ParseFileSystemFromSession(List<string> session)
        {
            var dirStack = new Stack<FileSystemDirectory>();
            var dirLookup = new Dictionary<string, FileSystemDirectory>();
            var root = new FileSystemDirectory("/");
            dirLookup["/"] = root;

            foreach (var line in session)
            {
                if (line.StartsWith("$ cd"))
                {
                    var param = line[5..];
                    if (param == "..")
                    {
                        dirStack.Pop();
                    }
                    else if (param == "/")
                    {
                        dirStack.Clear();
                        dirStack.Push(root);
                    }
                    else
                    {
                        var next = (FileSystemDirectory)dirStack.Peek().Get(param);
                        dirStack.Push(next);
                    }
                }
                else if (line == "$ ls")
                {
                    // Output pending
                }
                else if (line.StartsWith("dir"))
                {
                    var dir = new FileSystemDirectory(line[4..]);
                    dirStack.Peek().Add(dir);
                }
                else if (char.IsDigit(line[0]))
                {
                    var sp = line.Split(' ');
                    var size = int.Parse(sp[0]);
                    var name = sp[1];
                    var file = new FileSystemFile(name, size);
                    dirStack.Peek().Add(file);
                }
                else
                {
                    throw new Exception($"Unexpected output: {line}");
                }
            }

            return root;
        }

        private static void TraverseFileSystem(FileSystemDirectory dir, Action<FileSystemObject> act)
        {
            act(dir);

            foreach (var obj in dir.Content)
            {
                if (obj is FileSystemDirectory d)
                {
                    TraverseFileSystem(d, act);
                }
                else
                {
                    act(obj);
                }
            }
        }
    }

    internal abstract class FileSystemObject
    {
        public string Name { get; }

        public FileSystemObject(string name)
        {
            Name = name;
        }
    }

    internal class FileSystemDirectory : FileSystemObject
    {
        public List<FileSystemObject> Content { get; } = new List<FileSystemObject>();

        public int TotalSize
        {
            get
            {
                if (_totalSize == null)
                {
                    _totalSize = 0;
                    foreach (var obj in Content)
                    {
                        if (obj is FileSystemDirectory d)
                        {
                            _totalSize += d.TotalSize;
                        }
                        else
                        {
                            _totalSize += ((FileSystemFile)obj).Size;
                        }
                    }
                }

                return _totalSize.Value;
            }
        }
        private int? _totalSize = null;

        public FileSystemDirectory(string name)
            : base(name)
        {
        }

        public FileSystemObject Get(string name)
        {
            return Content.First(x => x.Name == name);
        }

        public void Add(FileSystemObject obj)
        {
            Content.Add(obj);
        }
    }

    internal class FileSystemFile : FileSystemObject
    {
        public int Size { get; }

        public FileSystemFile(string name, int size)
            : base(name)
        {
            Size = size;
        }
    }
}
