namespace CSharp;

public static class Program
{
    private record Coordinate(int X, int Y)
    {
        public (int, int) ToTuple => (X,Y);
        public int DistanceTo((int X, int Y) other) => 
            Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        public int DistanceTo(Coordinate other) => DistanceTo(other.ToTuple);
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

    private static void Print(HashSet<Coordinate> sensors, HashSet<Coordinate> beacons, (int,int)[] covers)
    {
        var coordinates = sensors.Concat(beacons);

        // var yMin = coordinates.Min(p => p.Y);
        // var yMax = coordinates.Max(p => p.Y);

        // var xMin = coordinates.Min(p => p.X);
        // var xMax = coordinates.Max(p => p.X);
        (var xMin, var xMax) = (-10, 20);
        (var yMin, var yMax) = (-5, 20);

        for (int y = yMin; y <= yMax; y++)
        {
            for (int x = xMin; x <= xMax; x++)
            {
                var current = new Coordinate(x,y);
                if (sensors.Contains(current))
                {
                    Console.Write('S');
                }
                else if (beacons.Contains(current))
                {
                    Console.Write('B');
                }
                else if(covers.Contains((x,y)))
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
    }

    private static IEnumerable<(int, int)> Covers(KeyValuePair<(int, int), Coordinate> kv)
    {
        var d = kv.Value.DistanceTo(kv.Key);

        (var xStart, var yStart) = kv.Key;

        // Console.WriteLine($"Starting from {kv.Key} | Distance: {d} | y: {yStart-d}-{yStart+d} | x: {xStart-d}-{xStart+d}");

        for (int y = yStart-2*d; y <= yStart+d; y++)
        {
            for (int x = xStart-2*d; x <= xStart+d; x++)
            {
                // Console.Write($" {kv.Value.DistanceTo((x,y)),2} ");
                if (kv.Value.DistanceTo((x,y)) <= d)
                {
                    // Console.Write((x,y));
                    yield return (x,y);
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

        // foreach (var kv in closestBeaconCoordinates)
        // {
        //     Console.WriteLine($"Distance to beacon at {kv.Key}: {kv.Value.DistanceTo(kv.Key)}");
        // }

        var covers = Covers(closestBeaconCoordinates.First()).ToArray();

        // Print(sensors, beacons, covers);

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
