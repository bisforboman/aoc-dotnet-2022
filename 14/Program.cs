namespace CSharp;

public static class Program
{
    private record Coordinate((int X, int Y) Point)
    {
        public Coordinate Down => new((Point.X, Point.Y+1));
        public Coordinate Left => new((Point.X-1, Point.Y));
        public Coordinate Right => new((Point.X+1, Point.Y));

        public override string ToString() => Point.ToString();
    };
    private record Line(Coordinate Start, Coordinate End)
    {
        public bool MovesInX => Start.Point.X != End.Point.X;
        public bool MovesInY => Start.Point.Y != End.Point.Y;
        public bool GrowsFromStart()
        {
            if (MovesInX)
            {
                return Start.Point.X < End.Point.X;
            }

            if (MovesInY)
            {
                return Start.Point.Y < End.Point.Y;
            }

            throw new Exception("not good! - GrowsFromStart");
        }
    };

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
            yield return new Line(Start, End);
        }
    }

    private static IEnumerable<Coordinate> ToCoordinates(this Line line)
    {
        int xStart = line.Start.Point.X, 
            xEnd = line.End.Point.X, 
            yStart = line.Start.Point.Y, 
            yEnd = line.End.Point.Y;

        if (!line.GrowsFromStart())
        {
            if (line.MovesInX)
            {
                xStart = line.End.Point.X;
                xEnd = line.Start.Point.X;
            }

            if (line.MovesInY)
            {
                yStart = line.End.Point.Y;
                yEnd = line.Start.Point.Y;
            }
        }

        if (line.MovesInX)
        {
            for (int x = xStart; x <= xEnd; x++)
            {
                yield return new((x,line.Start.Point.Y));
            }
        }
        
        if (line.MovesInY)
        {
            for (int y = yStart; y <= yEnd; y++)
            {
                yield return new((line.Start.Point.X, y));
            }
        }
    }

    private static void PrintCoordinates(this IEnumerable<Coordinate> rocks, IEnumerable<Coordinate> sands)
    {
        var occupied = rocks.Concat(sands);
        var xMin = occupied.Min(p => p.Point.X)-1;
        var xMax = occupied.Max(p => p.Point.X)+1;
        var yMin = occupied.Min(p => p.Point.Y)-1;
        var yMax = occupied.Max(p => p.Point.Y)+1;
        var DropPoint = new Coordinate((500,yMin)); 

        string nothing = " ";
        string rock = "\u2588";
        string sand = "\u2591";
        string drop = "+";

        for (int y = yMin; y <= yMax; y++)
        {
            for (int x = xMin; x <= xMax; x++)
            {
                var coord = new Coordinate((x,y));

                if (coord.Equals(DropPoint))
                {
                    Console.Write(drop);
                }
                else if (rocks.Contains(coord))
                {
                    Console.Write(rock);
                }
                else if (sands.Contains(coord))
                {
                    Console.Write(sand);
                }
                else
                {
                    Console.Write(nothing);
                }
            }
            Console.WriteLine();
        }
    }

    private static Coordinate? FindNextSpotForSand(IDictionary<(int, int), Coordinate> rocks, IDictionary<(int, int), Coordinate> sands, int yMax)
    {
        var occupied = rocks.Concat(sands).ToDictionary(x => x.Key);
        int x = 500, y = -1;
        while(y++ < yMax)
        {
            var current = new Coordinate((x,y));

            if (!occupied.ContainsKey(current.Down.Point))
            {
                continue;
            }

            if (!occupied.ContainsKey(current.Down.Left.Point))
            {
                x--;
                continue;
            }

            if (!occupied.ContainsKey(current.Down.Right.Point))
            {
                x++;
                continue;
            }

            return current;
        }

        return null;
    }

    private static IDictionary<(int, int), Coordinate> DropSand(this IDictionary<(int, int), Coordinate> rocks)
    {
        var yMax = rocks.Values.Max(p => p.Point.Y);
        var moreSands = new Dictionary<(int, int), Coordinate>();
        while(true)
        {
            var nextSand = FindNextSpotForSand(rocks, moreSands, yMax);

            if (nextSand is null)
            {
                return moreSands;
            }

            moreSands.Add(nextSand.Point, nextSand);

            if (nextSand == new Coordinate((500, 0)))
            {
                return moreSands;
            }
        }
    }

    private static void Part1(string inputPath)
    {
        var input = ReadAndParseInput(inputPath);

        var rocks = input
            .SelectMany(LinesOfRock)
            .SelectMany(ToCoordinates)
            .ToHashSet()
            .ToDictionary(x => x.Point);

        var sands = rocks.DropSand();

        var result = sands.Count;

        Console.WriteLine("Part1 result: " + result);
    }

    private static Coordinate? FindNextSpotForSand2(IDictionary<(int, int), Coordinate> occupied, int yMax)
    {
        int x = 500, y = -1;

        while(y++ < yMax)
        {
            var current = new Coordinate((x,y));

            if (!occupied.ContainsKey(current.Down.Point))
            {
                continue;
            }

            if (!occupied.ContainsKey(current.Down.Left.Point))
            {
                x--;
                continue;
            }

            if (!occupied.ContainsKey(current.Down.Right.Point))
            {
                x++;
                continue;
            }

            return current;
        }

        return null;
    }

    private static IDictionary<(int, int), Coordinate> FillWithSand(this IDictionary<(int,int), Coordinate> rocks)
    {
        var yMax = rocks.Values.Max(p => p.Point.Y);
        var moreSands = new Dictionary<(int, int), Coordinate>();
        var occupied = new Dictionary<(int, int), Coordinate>(rocks);
        while(true)
        {
            var nextSand = FindNextSpotForSand2(occupied, yMax);

            if (nextSand is null)
            {
                return moreSands;
            }

            occupied.Add(nextSand.Point, nextSand);
            moreSands.Add(nextSand.Point, nextSand);

            if (nextSand == new Coordinate((500, 0)))
            {
                return moreSands;
            }
        }

    }

    private static void Part2(string inputPath)
    {
        var input = ReadAndParseInput(inputPath);

        var rocks = input
            .SelectMany(LinesOfRock)
            .SelectMany(ToCoordinates)
            ;

        var y = rocks.Max(p => p.Point.Y)+2;
        var xMin = 500 - y;
        var xMax = 500 + y;

        var floor = new Line(new Coordinate((xMin, y)), new Coordinate((xMax, y)));

        var rocksWithFloor = rocks
            .Concat(floor.ToCoordinates())
            .ToHashSet()
            .ToDictionary(x => x.Point);

        var sands = rocksWithFloor.FillWithSand();

        PrintCoordinates(rocksWithFloor.Values, sands.Values);

        var result = sands.Count;

        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
    
}
