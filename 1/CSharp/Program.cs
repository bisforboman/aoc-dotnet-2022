namespace CSharp;

public static class Program
{
    private static IEnumerable<int> ReadAndParseInput(string filePath) => 
        File.ReadAllText(filePath)
            .Split("\n\r\n")
            .Select(ToIntArrays);

    private static int ToIntArrays(this string s) => 
        s.Split('\n')
         .Select(int.Parse)
         .Sum();

    private static void Part1(string inputPath)
    {
        var input = ReadAndParseInput(inputPath);

        var result = input.Max();

        Console.WriteLine("Part1 result: " + result);
    }

    private static void Part2(string inputPath)
    {
        var input = ReadAndParseInput(inputPath);

        var result = input
            .OrderByDescending(e => e)
            .Take(3)
            .Sum();

        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
}
