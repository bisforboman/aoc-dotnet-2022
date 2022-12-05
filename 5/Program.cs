namespace CSharp;

public static class Program
{    
    private static IEnumerable<string> ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath);

    private static char[][] ParseFirst(IEnumerable<string> ss)
    {
        
    }
        
    private static void Part1(string inputPath)
    {
        var input = ReadAndParseInput(inputPath);

        var first = input
            .TakeWhile(e => !e.Trim().StartsWith("1"));
        
        var second = input
            .SkipWhile(e => !e.Trim().StartsWith("1"))
            .Skip(2);

        var result = input.Count();
        Console.WriteLine("Part1 result: " + (first.Count(), second.Count()));
    }


    private static void Part2(string inputPath)
    {
        var input = ReadAndParseInput(inputPath);

        var result = input.Count();
        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
}
