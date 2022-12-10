namespace CSharp;

public static class Program3
{
    private static string[] ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath);

    private static (int,int) ToTuples(string s)
    {
        var ret = s.Split(' ');

        return ret[0] switch
        {
            "noop" => (1, 0),
            "addx" => (2, int.Parse(ret[1])),
            _ => throw new Exception("not good"),
        };
    }

    private static IEnumerable<(int, int)> ToCycles((int cycleCount, int value) Operation)
    {
        for (var i = 1; i < Operation.cycleCount; i++)
        {
            yield return (1, 0);
        }

        yield return (1, Operation.value);
    }

    private static IEnumerable<KeyValuePair<int, int>> Accumulate(IEnumerable<(int, int)> inputs)
    {
        int cycle = 1;
        var value = 1;
        foreach ((var c, var v) in inputs)
        {
            yield return KeyValuePair.Create(cycle, value);
            cycle += c;
            value += v;
        }
    }

    private static int ToSignalStrength(this Dictionary<int, int> kv, int index) => 
        index * kv[index];

    private static void Part1(string inputPath)
    {
        var inputs = ReadAndParseInput(inputPath)
            .Select(ToTuples)
            .SelectMany(ToCycles)
            ;

        var kvs = Accumulate(inputs)
            .ToDictionary(x => x.Key, x => x.Value);

        var result = 
            kvs.ToSignalStrength(20) +
            kvs.ToSignalStrength(60) +
            kvs.ToSignalStrength(100) +
            kvs.ToSignalStrength(140) +
            kvs.ToSignalStrength(180) +
            kvs.ToSignalStrength(220);

        Console.WriteLine("Part1 result: " + result);
    }

    private static bool SpriteCovers(this int spritePosition, int index) => 
        (index - spritePosition) is 2 or 1 or 0;

    private static void Part2(string inputPath)
    {
        var inputs = ReadAndParseInput(inputPath)
            .Select(ToTuples)
            .SelectMany(ToCycles)
            ;

        var spritePositions = Accumulate(inputs)
            .ToDictionary(x => x.Key, x => x.Value);

        for (var rows = 0; rows < 6; rows++)
        {
            for (var cycle = 1; cycle <= 40; cycle++)
            {
                var val = cycle + rows * 40;
                if (spritePositions[val].SpriteCovers(cycle))
                {
                    Console.Write("\u2588");
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
    
}
