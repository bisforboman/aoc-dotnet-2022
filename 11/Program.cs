namespace CSharp;

public static class Program
{
    private static string[] ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath);

    private static void Operate(this Dictionary<int, Monkey> monkeys)
    {
        foreach (var m in monkeys.Values)
        {
            var throws = m.Throw().ToArray();
            foreach ((var index, var item) in throws)
            {
                monkeys[index].Items.Add(item);
            }
        }
    }

    private static UInt128 GetMonkeyBusiness(this Dictionary<int, Monkey> monkeys)
    {
        var monkeyBusiness = monkeys.Values
            .Select(m => m.Inspects)
            .OrderByDescending(m => m)
            .Take(2)
            .ToArray();

        return monkeyBusiness[0] * monkeyBusiness[1];
    }

    private static UInt128 GetReduceableValue(this Dictionary<int, Monkey> monkeys)
    {   
        UInt128 reducableValue = 1;
        foreach (var m in monkeys.Values.Select(m => m.IsDivisibleBy))
        {
            reducableValue *= m;
        }

        return reducableValue;
    }

    private static void OperateAndReduce(this Dictionary<int, Monkey> monkeys, UInt128 reduceBy)
    {
        foreach (var m in monkeys.Values)
        {
            var throws = m.Throw().ToArray();
            foreach ((var index, var item) in throws)
            {
                monkeys[index].Items.Add(item % reduceBy);
            }
        }
    }

    private static IEnumerable<string[]> Separate(this string[] ss)
    {
        for (int i = 0; i < (ss.Length+1) / 7; i++)
        {
            yield return ss.Skip(i * 7).Take(6).ToArray();
        }
    }

    private static void Part1(string inputPath)
    {
        var inputs = ReadAndParseInput(inputPath)
            .Separate()
            .Select(Monkey.Create)
            .Select(m => Monkey.WithWorryLevelReducer(m, 3))
            .ToDictionary(m => m.Index, m => m)
            ;

        var monkeys = inputs;

        for (int i = 0; i < 20; i++)
        {
            monkeys.Operate();
        }

        var result = monkeys.GetMonkeyBusiness();

        Console.WriteLine("Part1 result: " + result);
    }

    private static void Part2(string inputPath)
    {
        var inputs = ReadAndParseInput(inputPath)
            .Separate()
            .Select(Monkey.Create)
            .ToDictionary(m => m.Index, m => m);

        var monkeys = inputs;
        var reducableValue = monkeys.GetReduceableValue();

        for (int i = 0; i < 10000; i++)
        {
            monkeys.OperateAndReduce(reducableValue);
        }
        
        var result = monkeys.GetMonkeyBusiness();

        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
    
}
