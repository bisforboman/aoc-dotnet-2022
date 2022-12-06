namespace CSharp;

public static class Program
{    
    private static IEnumerable<string> ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath)
            ;

    private static IEnumerable<(int, string)> ToChunks(this string s, int chunkSize)
    {
        foreach ((var c, var i) in s.Select((c,i) => (c,i)))
        {
            yield return (i+chunkSize, s.Substring(i, chunkSize));
        }
    }

    private static void Part1(string inputPath)
    {
        var inputs = ReadAndParseInput(inputPath);

        var results = inputs.Select(i => i
            .ToChunks(4)
            .First(i => i.Item2.Distinct().Count() == 4));

        var result = string.Join("\n", results);

        Console.WriteLine("Part1 result: " + result);
    }

    private static void Part2(string inputPath)
    {
        var inputs = ReadAndParseInput(inputPath);

        var results = inputs.Select(i => i
            .ToChunks(14)
            .First(i => i.Item2.Distinct().Count() == 14));

        var result = string.Join("\n", results);

        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
}
