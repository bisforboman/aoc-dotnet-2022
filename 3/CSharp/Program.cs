namespace CSharp;

public static class Program
{
    private static IEnumerable<string> ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath);

    private static (string, string) Separate(string s)
    {
        var half = s.Length / 2;
        return (s[..half], s[half..]);
    }

    private static int GetPriority(char c)
    {
        if (c is >= 'A' and <= 'Z')
        {
            return c - (65 - 27);
        }

        if (c is >= 'a' and <= 'z')
        {
            return c - 96;
        }

        throw new ArgumentException(c.ToString());
}

    private static void Part1(string inputPath)
    {
        var input = ReadAndParseInput(inputPath)
            .Select(Separate);

        var result = input.SelectMany(tu => tu.Item1.Intersect(tu.Item2))
            .Select(GetPriority)
            .Sum();

        Console.WriteLine("Part1 result: " + result);
    }

    private static IEnumerable<(string, string, string)> GroupsOfThree(string[] ss)
    {
        for (int i = 0; i < ss.Length; i += 3)
        {
            yield return (ss[i], ss[i+1], ss[i+2]);
        }
    }

    private static void Part2(string inputPath)
    {
        var input = ReadAndParseInput(inputPath)
            .ToArray();

        var groupsOf3 = GroupsOfThree(input);

        var result = groupsOf3
            .SelectMany(tu => tu.Item1
                .Intersect(tu.Item2)
                .Intersect(tu.Item3))
            .Select(GetPriority)
            .Sum();

        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
}
