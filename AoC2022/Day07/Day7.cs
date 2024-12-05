using System.Text.RegularExpressions;

namespace AoC2022
{
    public class Day7 : AoC.DayBase
    {
        internal record class DirInfo(string Name, DirInfo? Parent = null)
        {
            public Dictionary<string, long> Files { get; } = new();
            public Dictionary<string, DirInfo> Directories { get; } = new();

            public long? totalSizeCache = null;

            public long TotalSize
            {
                get
                {
                    if (totalSizeCache != null)
                        return totalSizeCache.Value;

                    totalSizeCache = Files.Sum(f => f.Value) + Directories.Sum(d => d.Value.TotalSize);
                    return totalSizeCache.Value;
                }
            }

            public IEnumerable<DirInfo> AllDirectories
            {
                get
                {
                    return Directories.Values.Concat(Directories.SelectMany(d => d.Value.AllDirectories));
                }
            }

            public void Print(int indent)
            {
                Console.WriteLine(new string(' ', indent) + $"- {Name} (dir, size={TotalSize})");

                foreach (var dir in Directories.Values)
                {
                    dir.Print(indent + 2);
                }

                foreach (var file in Files)
                {
                    Console.WriteLine(new string(' ', indent + 2) + $"- {file.Key} (file, size={file.Value})");
                }
            }
        };

        private DirInfo SimulateCommands(string filename)
        {
            var root = new DirInfo("/", null);

            DirInfo current = root;

            foreach (var line in System.IO.File.ReadAllLines(filename))
            {
                var matchDir = Regex.Match(line, @"dir (.+)");
                if (matchDir.Success)
                {
                    var dirName = matchDir.Groups[1].Value;

                    current.Directories.Add(dirName, new DirInfo(dirName, current));
                }

                var matchFile = Regex.Match(line, @"(\d+) (.+)");
                if (matchFile.Success)
                {
                    var fileSize = int.Parse(matchFile.Groups[1].Value);
                    var fileName = matchFile.Groups[2].Value;

                    current.Files.Add(fileName, fileSize);
                }

                var matchCd = Regex.Match(line, @"\$ cd (.+)");
                if (matchCd.Success)
                {
                    var targetDir = matchCd.Groups[1].Value;

                    if (targetDir == "/")
                    {
                        current = root;
                    }
                    else if (targetDir == "..")
                    {
                        if (current.Parent == null)
                        {
                            throw new InvalidOperationException();
                        }

                        current = current.Parent;
                    }
                    else
                    {
                        current = current.Directories[targetDir];
                    }
                }
            }

            return root;
        }

        protected override object Solve1(string filename)
        {
            var root = SimulateCommands(filename);

            return root.AllDirectories.Select(d => d.TotalSize).Where(s => s <= 100000).Sum();
        }

        protected override object Solve2(string filename)
        {
            var root = SimulateCommands(filename);

            var unused = 70000000 - root.TotalSize;
            var mustFree = 30000000 - unused;

            return root.AllDirectories.Select(d => d.TotalSize).Where(s => s >= mustFree).Min();
        }

        public override object SolutionExample1 => 95437L;
        public override object SolutionPuzzle1 => 1723892L;
        public override object SolutionExample2 => 24933642L;
        public override object SolutionPuzzle2 => 8474158L;
    }
}
