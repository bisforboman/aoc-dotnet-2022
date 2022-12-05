namespace CSharp;

public static class Program
{    
    private static IEnumerable<string> ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath);
        
    private static void Part1(string inputPath)
    {
        var input = ReadAndParseInput(inputPath);

        var result = input.Count();
        Console.WriteLine("Part1 result: " + result);
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
