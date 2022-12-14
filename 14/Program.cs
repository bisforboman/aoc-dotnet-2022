namespace CSharp;

public static class Program
{
    private record Coordinate((int X, int Y) Point);
    private record Line(Coordinate Start, Coordinate End);

    private static string[] ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath);

    private static Coordinate ToCoordinate(string s)
    {
        var apa = s.Split(',');

        return new((int.Parse(apa[0]), int.Parse(apa[1])));
    }

    private static IEnumerable<Line> LinesOfRock(this string row)
    {
        var coords = row.Split(" -> ").Select(ToCoordinate);
        var zipped = Enumerable.Zip(coords.Prepend(new((0,0))), coords).Skip(1);

        foreach (var (Start, End) in zipped)
        {
            yield return new(Start, End);
        }
    }

    private static (int, int) GetXCoords(IEnumerable<Line> linesOfRock)
    {
        var xMax_Start = linesOfRock.Max(l => l.Start.Point.X);
        var xMax_End = linesOfRock.Max(l => l.End.Point.X);

        var xMax = xMax_Start > xMax_End 
            ? xMax_Start 
            : xMax_End;

        var xMin_Start = linesOfRock.Min(l => l.Start.Point.X);
        var xMin_End = linesOfRock.Min(l => l.End.Point.X);

        var xMin = xMin_Start < xMin_End 
            ? xMin_Start
            : xMin_End;

        return (xMin, xMax);
    }

    private static (int, int) GetYCoords(IEnumerable<Line> linesOfRock)
    {
        var yMax_Start = linesOfRock.Max(l => l.Start.Point.Y);
        var yMax_End = linesOfRock.Max(l => l.End.Point.Y);

        var yMax = yMax_Start > yMax_End 
            ? yMax_Start 
            : yMax_End;

        var yMin_Start = linesOfRock.Min(l => l.Start.Point.Y);
        var yMin_End = linesOfRock.Min(l => l.End.Point.Y);

        var yMin = yMin_Start < yMin_End 
            ? yMin_Start
            : yMin_End;

        return (yMin, yMax);
    }

    private static void Part1(string inputPath)
    {
        var input = ReadAndParseInput(inputPath);

        var linesOfRock = input.SelectMany(LinesOfRock);

        foreach (var l in linesOfRock)
        {
            Console.WriteLine(l);
        }

        (var xMin, var xMax) = GetXCoords(linesOfRock);
        (var yMin, var yMax) = GetYCoords(linesOfRock);

        for (int y = yMin; y <= yMax; y++)
        {
            // write header
            if (y == yMin)
            {
                for (int x = xMin; x <= xMax; x++)
                {
                    if (x == xMin)
                    {
                        Console.Write("    ");
                    }
                    Console.Write($"{x,4}");
                }
                Console.WriteLine();
            }

            for (int x = xMin-1; x <= xMax; x++)
            {
                if (x == xMin-1)
                {
                    Console.Write($"{y,4}");
                    continue;
                }
                Console.Write("  . ");
            }
            Console.WriteLine();
        }

        var result = "";

        Console.WriteLine("Part1 result: " + result);
    }

    private static void Part2(string inputPath)
    {
        var input = ReadAndParseInput(inputPath)
            ;

        var result = "";

        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
    
}
