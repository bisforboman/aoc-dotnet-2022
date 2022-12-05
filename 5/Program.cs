namespace CSharp;

public static class Program
{    
    private static IEnumerable<string> ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath);

    private static IEnumerable<char> ParseOne(this IEnumerable<string> ss, int index) => 
        ss.Select(s => s.ElementAt(index))
          .Where(s => !string.IsNullOrWhiteSpace(s.ToString()));

    private static IEnumerable<IEnumerable<char>> ParseFirst(IEnumerable<string> ss, int[] indexes) => 
        indexes.Select(ss.ParseOne);

    private static Stack<char>[] ParseState(IEnumerable<string> input)
    {
        var first = input
            .TakeWhile(e => !e.Trim().StartsWith("1"));

        var indexes = input
            .ElementAt(first.Count())
            .Select((x,i) => (Item: x, Index: i))
            .Where(tu => tu.Item != ' ')
            .Select(i => i.Index)
            .ToArray();

        return ParseFirst(first, indexes)
            .Select(s => new Stack<char>(s.Reverse())).ToArray();
    }

    private static int ParseAt(this IEnumerable<string> s, int index) =>
        int.Parse(s.ElementAt(index));

    private static IEnumerable<(int, int, int)> ParseInstructions(IEnumerable<string> input) => 
        input.SkipWhile(e => !e.Trim().StartsWith("1"))
            .Skip(2)
            .Select(l => l.Split(' '))
            .Select(ins => (ins.ParseAt(1), ins.ParseAt(3)-1, ins.ParseAt(5)-1));

    private static void Operate(this Stack<char>[] state, int amount, int from, int to)
    {
        for (int i=0; i < amount; i++)
        {
            state[to].Push(state[from].Pop());
        }
    }

    private static void Part1(string inputPath)
    {
        var input = ReadAndParseInput(inputPath);

        var state = ParseState(input);
        var instructions = ParseInstructions(input);

        foreach ((var amount, var from, var to) in instructions)
        {
            state.Operate(amount, from, to);
        }

        var topState = state.Select(s => s.Peek());

        var result = string.Join("", topState);

        Console.WriteLine("Part1 result: " + result);
    }

    private static void Operate2(this Stack<char>[] state, int amount, int from, int to)
    {
        var list = new List<char>();
        for (int i=0; i < amount; i++)
        {
            list.Add(state[from].Pop());
        }

        list.Reverse();

        for (int i=0; i < list.Count; i++)
        {
            state[to].Push(list[i]);
        }
    }


    private static void Part2(string inputPath)
    {
        var input = ReadAndParseInput(inputPath);

        var state = ParseState(input);
        var instructions = ParseInstructions(input);

        foreach ((var amount, var from, var to) in instructions)
        {
            state.Operate2(amount, from, to);
        }

        var topState = state.Select(s => s.Peek());

        var result = string.Join("", topState);

        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
}
