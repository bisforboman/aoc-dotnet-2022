namespace CSharp;

public static class Program3
{
    private static string[] ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath);

    private static IEnumerable<(int, int)> TranslateInstruction((string Direction, int Amount) Tuple) => Tuple.Direction switch
    {
        "R" => Enumerable.Repeat((1, 0), Tuple.Amount),
        "L" => Enumerable.Repeat((-1, 0), Tuple.Amount),
        "U" => Enumerable.Repeat((0, -1), Tuple.Amount),
        "D" => Enumerable.Repeat((0, 1), Tuple.Amount),
        _ => throw new Exception("not good")
    };

    private static (int, int) MoveToBecomeNeighbor(this (int X, int Y) a, (int X, int Y) b)
    {
        var xFar = Math.Abs(a.X - b.X) == 2;
        var yFar = Math.Abs(a.Y - b.Y) == 2;
        var updatedX = a.X + (b.X - a.X)/2;
        var updatedY = a.Y + (b.Y - a.Y)/2;

        if (xFar && yFar)
        {
            return (updatedX, updatedY);
        }
        else if (xFar)
        {
            return (updatedX, b.Y);
        }
        else if (yFar)
        {
            return (b.X, updatedY);
        }

        return a;
    }

    private static IEnumerable<(int, int)> GetTailCoordinates(IEnumerable<IEnumerable<(int, int)>> moves, (int X, int Y)[] rope)
    {
        foreach (var directions in moves)
        {
            foreach ((var x, var y) in directions)
            {
                rope[0].X += x;
                rope[0].Y += y;
                for (var i = 1; i < rope.Length; i++)
                {
                    rope[i] = rope[i].MoveToBecomeNeighbor(rope[i-1]);
                }

                yield return rope[^1];
            }
        }
    }

    private static (string,int) ToTuples(string s)
    {
        var ret = s.Split(' ');
        return (ret[0], int.Parse(ret[1]));
    }

    private static void Part1(string inputPath)
    {
        var moves = ReadAndParseInput(inputPath)
            .Select(ToTuples)
            .Select(TranslateInstruction);

        var rope = new (int, int)[] { (0,0), (0,0) };

        var tailCoords = GetTailCoordinates(moves, rope).ToHashSet();

        var result = tailCoords.Count;

        Console.WriteLine("Part1 result: " + result);
    }

    private static void Part2(string inputPath)
    {
        var moves = ReadAndParseInput(inputPath)
            .Select(ToTuples)
            .Select(TranslateInstruction);

        var rope = Enumerable.Repeat((0,0), 10).ToArray();

        var tailCoords = GetTailCoordinates(moves, rope).ToHashSet();

        var result = tailCoords.Count;

        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
    
}
