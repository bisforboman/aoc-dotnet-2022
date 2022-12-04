namespace CSharp;

public static class Program
{    
    private static IEnumerable<string> ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath);

    private static (string, string) TupleSplit(this string s, char splitCondition)
    {
        var sep = s.Split(splitCondition);

        return (sep[0], sep[1]);
    }

    private static (int, int) ToDiscreteValues(string s)
    {
        (var a, var b) = s.TupleSplit('-');

        return (int.Parse(a), int.Parse(b));
    }

    private static bool IsContainedIn((int, int) a, (int, int) b) => 
        (a.Item1 <= b.Item1 && a.Item2 >= b.Item2) || 
        (a.Item1 >= b.Item1 && a.Item2 <= b.Item2);
        
    private static void Part1(string inputPath)
    {
        var input = ReadAndParseInput(inputPath)
            .Select(s => TupleSplit(s, ','))
            .Select(tu => (ToDiscreteValues(tu.Item1), ToDiscreteValues(tu.Item2)))
            .Where(tu => IsContainedIn(tu.Item1, tu.Item2));

        Console.WriteLine("Part1 result: " + input.Count());
    }

    private static IEnumerable<int> ToDiscreteValues2(string s)
    {
        (var a, var b) = s.TupleSplit('-');

        var lower = int.Parse(a);
        var upper = int.Parse(b);

        for (var i = lower; i <= upper; i++)
        {
            yield return i;
        }
    }

    private static bool IsContainedIn2(IEnumerable<int> a, IEnumerable<int> b) => 
        a.Intersect(b).Any();

    private static void Part2(string inputPath)
    {
        var input = ReadAndParseInput(inputPath)
            .Select(s => TupleSplit(s, ','))
            .Select(tu => (ToDiscreteValues2(tu.Item1), ToDiscreteValues2(tu.Item2)))
            .Where(tu => IsContainedIn2(tu.Item1, tu.Item2));

        Console.WriteLine("Part2 result: " + input.Count());
    }

    private static UInt128 ToDiscreteValues3(string s)
    {
        (var a, var b) = s.TupleSplit('-');

        var lower = int.Parse(a)-1;
        var upper = int.Parse(b)-1;

        UInt128 ret = 0;
        for (var i = lower; i <= upper; i++)
        {
            ret += UInt128.One << i;
        }

        return ret;
    }

    private static void Part2_2(string inputPath)
    {
        var input = ReadAndParseInput(inputPath)
            .Select(s => TupleSplit(s, ','))
            .Select(tu => (ToDiscreteValues3(tu.Item1), ToDiscreteValues3(tu.Item2)))
            .Where(tu => (tu.Item1 & tu.Item2) > 0);

        Console.WriteLine("Part2_2 result: " + input.Count());
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
        Part2_2(args[0]);
    }
}
