namespace CSharp;

public static class Program
{
    private record Coordinate(int X, int Y)
    {
        public (int, int) ToTuple => (X,Y);
        public static Coordinate Create((int x,int y) p) => new(p.x,p.y);
        public int DistanceTo((int X, int Y) other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        public int DistanceTo(Coordinate other) => DistanceTo(other.ToTuple);
        public long TuningFrequency => ((long)X) * 4000000 + Y;
    };

    private static string[] ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath);

    private static (Coordinate, Coordinate) ToCoordinates(this string s)
    {
        var ss = s.Replace("Sensor at ", string.Empty)
            .Replace("x=", string.Empty)
            .Replace("y=", string.Empty)
            .Split(": closest beacon is at ");

        var left = ss[0].Split(',');
        var right = ss[1].Split(',');

        (var sensorX, var sensorY) = (int.Parse(left[0]), int.Parse(left[1]));
        (var beaconX, var beaconY) = (int.Parse(right[0]), int.Parse(right[1]));

        return (new(sensorX, sensorY), new(beaconX, beaconY));
    }

    private static void Print(IEnumerable<(Coordinate, Coordinate)> coordinates, HashSet<Coordinate> covers, (int Min, int Max) xs, (int Min, int Max) ys)
    {
        var yMin = ys.Min;
        var yMax = ys.Max;
        var xMin = xs.Min;
        var xMax = xs.Max;

        var sensors = coordinates.Select(e => e.Item1).ToHashSet();
        var beacons = coordinates.Select(e => e.Item2).ToHashSet();

        for (int y = yMin-1; y <= yMax; y++)
        {
            for (int x = xMin; x <= xMax; x++)
            {
                var current = new Coordinate(x,y);
                if (sensors.Contains(current))
                {
                    Console.Write("S");
                }
                else if (beacons.Contains(current))
                {
                    Console.Write("B");
                }
                else if(covers.Contains(current))
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }
    }

    private static IEnumerable<Coordinate> Covers(KeyValuePair<(int, int), Coordinate> kv, (int Start, int End) search)
    {
        var sensorCoord = Coordinate.Create(kv.Key);

        var d = kv.Value.DistanceTo(sensorCoord);

        (var xStart, var yStart) = search;

        for (int y = yStart-d; y <= yStart+d; y++)
        {
            for (int x = xStart-d; x <= xStart+d; x++)
            {
                if (sensorCoord.DistanceTo((x,y)) <= d)
                {
                    yield return new Coordinate(x,y);
                }
            }
        }
    }

    private static void Part1(string inputPath)
    {
        var input = ReadAndParseInput(inputPath);

        var coordinates = input.Select(ToCoordinates);

        var closestBeaconCoordinates = coordinates
            .ToDictionary(c => c.Item1.ToTuple, c => c.Item2);
        
        var sensors = coordinates
            .Select(c => c.Item1)
            .ToHashSet();
        
        var beacons = coordinates
            .Select(c => c.Item2)
            .ToHashSet();

        var y = 10;
        var covered = new HashSet<Coordinate>();

        foreach (var b in closestBeaconCoordinates)
        {
            foreach (var c in Covers(b, b.Key))
            {
                if (!beacons.Contains(c))
                { 
                    covered.Add(c);
                }
            }
        }

        var xMin = covered.Min(p => p.X);
        var xMax = covered.Max(p => p.X);

        var contained = new HashSet<Coordinate>();

        for (int x = xMin; x <= xMax; x++)
        {
            var coord = new Coordinate(x,y);
            if (covered.Contains(coord))
            {
                contained.Add(coord);
            }
        }

        var result = contained.Count;

        Console.WriteLine("Part1 result: " + result);
    }

    private static void Part2(string inputPath)
    {
        var input = ReadAndParseInput(inputPath);

        var coordinates = input.Select(ToCoordinates);

        var sensors = coordinates
            .Select(c => c.Item1)
            .ToHashSet();
        
        var beacons = coordinates
            .Select(c => c.Item2)
            .ToHashSet();

        var circles = coordinates
            .Select(c => (Coord: c.Item1, Distance: c.Item1.DistanceTo(c.Item2)))
            .ToArray();

        int min = 0, max = 4000000;

        var map = new HashSet<Coordinate>();
        for (int i = 0; i < circles.Length; i++)
        {
            var c = circles[i];

            for (int j = i+1; j < circles.Length; j++)
            {                
                var d = circles[j];

                if (c.Coord.DistanceTo(d.Coord) == c.Distance + d.Distance + 2)
                {
                    int yMax = Math.Min(c.Coord.Y + c.Distance, d.Coord.Y + d.Distance);
                    int yMin = Math.Max(c.Coord.Y - c.Distance, d.Coord.Y - d.Distance);
                    
                    int xMin = Math.Max(c.Coord.X - c.Distance, d.Coord.X - d.Distance);
                    int xMax = Math.Min(c.Coord.X + c.Distance, d.Coord.X + d.Distance);
                    
                    for (int y = yMin; y < yMax; y++)
                    {
                        int xa = c.Coord.X + (c.Distance + 1 - Math.Abs(y - c.Coord.Y));                    
                        if (min <= xa && xa <= max && xMin <= xa && xa <= xMax)
                        {
                            map.Add(new Coordinate(xa, y));
                        }

                        int xb = c.Coord.X - (c.Distance + 1 - Math.Abs(y - c.Coord.Y));
                        if (min <= xb && xb <= max && xMin <= xb && xb <= xMax)
                        {
                            map.Add(new Coordinate(xb, y));
                        }
                    }
                }
            }
        }
        
        var coord = FindCoord(sensors, beacons, circles, map);

        var result = coord.TuningFrequency;

        Console.WriteLine("Part2 result: " + result);
    }

    private static Coordinate FindCoord(HashSet<Coordinate> sensors, HashSet<Coordinate> beacons, (Coordinate, int)[] circles, HashSet<Coordinate> map)
    {
        foreach (var point in map)
        {
            if (sensors.Contains(point) || beacons.Contains(point))
            {
                continue;
            }

            bool found = true;
            foreach (var c in circles)
            {
                if (point.DistanceTo(c.Item1) <= c.Item2)
                {
                    found = false;
                    break;
                }
            }

            if (found)
            {
                return point;
            }
        }

        throw new Exception("nope");
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
    
}
