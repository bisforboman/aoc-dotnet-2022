namespace CSharp;

public static class Program
{
    private static string[] ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath)
            .Skip(1)
            .ToArray();

    private static (string[], string[]) SplitByPrefix(this IEnumerable<string> ss, string splitter) => 
        (ss.TakeWhile(s => !s.StartsWith(splitter)).ToArray(), ss.SkipWhile(s => !s.StartsWith(splitter)).ToArray());

    private static IEnumerable<(long, string)> GetFiles(this IEnumerable<string> ss)
    {
        foreach (var s in ss.Where(s => !s.StartsWith("dir")))
        {
            var tu = s.Split(' ');
            yield return (long.Parse(tu[0]), tu[1]);
        }
    }

    private static IEnumerable<string> GetDirs(this IEnumerable<string> ss)
    {
        foreach (var s in ss.Where(s => s.StartsWith("dir")))
        {
            var tu = s.Split(' ');
            yield return tu[1];
        }
    }

    private static void CreateDirectories(DirItem currentDir, string[] instructions)
    {
        if (!instructions.Any())
        {
            return;
        }

        var action = instructions[0];
        var restOfInstructions = instructions[1..];

        if (action is "$ ls")
        {
            (var contents, var rest) = restOfInstructions.SplitByPrefix("$");

            // Console.WriteLine("Listing objects ..");
            foreach (var f in contents.GetFiles())
            {
                var file = new FileItem(f.Item2, f.Item1, currentDir);
                currentDir.Files.Add(file);
                // Console.WriteLine($"- Added {file}");
            }

            foreach (var d in contents.GetDirs())
            {
                var dir = new DirItem(d, currentDir);
                currentDir.Directories.Add(d, dir);
                // Console.WriteLine($"- Added {dir}");
            }

            CreateDirectories(currentDir, rest);
            return;
        }

        if (action.StartsWith("$ cd .."))
        {
            // Console.WriteLine("Moving up!");
            if (currentDir.Parent is not null)
            {
                CreateDirectories(currentDir.Parent, restOfInstructions);
                return;
            }

            throw new Exception("parent null");
        }

        if (action.StartsWith("$ cd"))
        {
            var whatDir = action.Split(' ').ElementAt(2);

            // Console.WriteLine("Moving into: " + whatDir);
            // Console.WriteLine("Dics: " + string.Join(",", currentDir.Directories.Select(e => e.Key)));

            CreateDirectories(currentDir.Directories[whatDir], restOfInstructions);
            return;
        }

        throw new Exception("missing action");
    }

    private static void Part1(string inputPath)
    {
        var inputs = ReadAndParseInput(inputPath);

        var dir = new DirItem("/", null);

        CreateDirectories(dir, inputs);

        var results = dir
            .Where(s => s.GetSize() < 100000);

        Console.WriteLine("Part1 result: " + results.Sum(r => r.GetSize()));
    }

    private static void Part2(string inputPath)
    {
        var inputs = ReadAndParseInput(inputPath);

        var dir = new DirItem("/", null);

        CreateDirectories(dir, inputs);

        var requires = dir.GetSize() - (70000000 - 30000000);

        var result = dir
            .Select(d => (d.Name, Size: d.GetSize()))
            .OrderBy(d => d.Size)
            .First(d => d.Size > requires);

        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
    
}
