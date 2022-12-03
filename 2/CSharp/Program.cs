namespace CSharp;

public static class Program
{
    private enum Action
    {
        Rock,
        Paper,
        Scissors,
        Draw,
        Win,
        Lose,
    }

    private static IEnumerable<string> ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath);

    private static (Action, Action) MapActions(string s, Func<string, Action> func)
    {        
        var derp = s.Split(' ');

        return (func(derp[0]), func(derp[1]));
    }

    private static Action Map(string s) => s switch
    {
        "A" or "X" => Action.Rock,
        "B" or "Y" => Action.Paper,
        "C" or "Z" => Action.Scissors,
        _ => throw new ArgumentException(s),
    };

    private static int WinOrLoss(Action opponent, Action you) => (opponent, you) switch
    {
        (Action.Rock, Action.Paper) => 6,
        (Action.Paper, Action.Scissors) => 6,
        (Action.Scissors, Action.Rock) => 6,

        // draw case
        _ when opponent == you => 3,

        _ => 0,
    };

    private static int InheritValue(Action a) => a switch
    {
        Action.Rock     => 1,
        Action.Paper    => 2,
        Action.Scissors => 3,
        _ => throw new ArgumentException(a.ToString()),
    };

    private static int Outcome(Action opponent, Action you) => 
        WinOrLoss(opponent, you) + InheritValue(you);

    private static void Part1(string inputPath)
    {
        var input = ReadAndParseInput(inputPath)
            .Select(s => MapActions(s, Map));

        var result = input
            .Select(t => Outcome(t.Item1, t.Item2))
            .Sum();

        Console.WriteLine("Part1 result: " + result);
    }

    private static Action Map2(string s) => s switch
    {
        "A" => Action.Rock,
        "B" => Action.Paper,
        "C" => Action.Scissors,
        "X" => Action.Lose,
        "Y" => Action.Draw,
        "Z" => Action.Win,
        _ => throw new ArgumentException(s),
    };

    private static Action InferAction((Action, Action) input) => input switch
    {
        (var a, Action.Draw) => a,

        (Action.Paper, Action.Win) => Action.Scissors,
        (Action.Rock, Action.Win) => Action.Paper,
        (Action.Scissors, Action.Win) => Action.Rock,

        (Action.Paper, Action.Lose) => Action.Rock,
        (Action.Rock, Action.Lose) => Action.Scissors,
        (Action.Scissors, Action.Lose) => Action.Paper,

        _ => throw new ArgumentException($"{input.Item1} {input.Item2}"),
    };

    private static void Part2(string inputPath)
    {
        var input = ReadAndParseInput(inputPath)
            .Select(s => MapActions(s, Map2));

        var afterInferredAction = input
            .Select(i => (i.Item1, InferAction(i)));

        var result = afterInferredAction
            .Select(t => Outcome(t.Item1, t.Item2))
            .Sum();

        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
}
