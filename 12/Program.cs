namespace CSharp;

public static class Program
{
    private static int[][] ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath)
            .Select(i => i.Select(j => 
            { 
                // a = 49
                if (j is 'S')
                {
                    return 48;
                }

                // z = 74
                if (j is 'E')
                {
                    return 75;
                }

                return j - '0';
            }).ToArray())
            .ToArray();

    private static bool CheckStep(this int[][] graph, (int X, int Y) current, (int X, int Y) next)
    {
        if (!(0 <= next.Y && next.Y < graph.Length))
        {
            return false;
        }

        if (!(0 <= next.X && next.X < graph[0].Length))
        {
            return false;
        }

        return graph[current.Y][current.X] + 1 >= graph[next.Y][next.X];
    }

    private static (int, int) SearchForExit(this int[][] graph)
    {
        for (int y = 0; y < graph.Length; y++)
        {
            for (int x = 0; x < graph[0].Length; x++)
            {
                if (graph[y][x] is 75)
                {
                    return (x,y);
                }
            }
        }

        throw new Exception("not good, no exit!");
    }

    private static (int, int) SearchForStart(this int[][] graph)
    {
        for (int y = 0; y < graph.Length; y++)
        {
            for (int x = 0; x < graph[0].Length; x++)
            {
                if (graph[y][x] is 48)
                {
                    return (x,y);
                }
            }
        }

        throw new Exception("not good, no start!");
    }

    private static IEnumerable<(int, int)> GetNextElevations(this int[][] graph, (int X, int Y) c)
    {
        if (graph.CheckStep((c.X, c.Y), (c.X, c.Y+1)))
        {
            yield return (c.X, c.Y+1);
        }

        if (graph.CheckStep((c.X, c.Y), (c.X, c.Y-1)))
        {
            yield return (c.X, c.Y-1);
        }

        if (graph.CheckStep((c.X, c.Y), (c.X+1, c.Y)))
        {
            yield return (c.X+1, c.Y);
        }

        if (graph.CheckStep((c.X, c.Y), (c.X-1, c.Y)))
        {
            yield return (c.X-1, c.Y);
        }
    }

    private static void PrintFlow(this int[][] graph, (int, int) exit, Dictionary<(int, int), (int, int)[]> paths)
    {
        var exitSpot = paths[exit];
        var derp = exitSpot.Prepend((0,0))
            .Zip(exitSpot, (f,s) => 
        {
            if (f.Item1 + 1 == s.Item1)
            {
                return (f, ">");
            }

            if (f.Item2 + 1 == s.Item2)
            {
                return (f, "v");
            }

            if (f.Item1 - 1 == s.Item1)
            {
                return (f, "<");
            }

            if (f.Item2 - 1 == s.Item2)
            {
                return (f, "^");
            }

            return (f, ".");
        }).ToDictionary(x => x.f, x => x.Item2);

        for (int y = 0; y < graph.Length; y++)
        {
            for (int x = 0; x < graph[0].Length; x++)
            {
                if (derp.TryGetValue((x,y), out var value))
                {
                    Console.Write(value);
                }
                else
                {
                    if ((x,y) == exit)
                    {
                        Console.Write("E");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
            }
            Console.WriteLine();
        }
    }

    private static Dictionary<(int, int), (int, int)[]> FindPaths(this int[][] graph, (int, int) start, (int, int) exit)
    {
        var paths = new Dictionary<(int, int), (int, int)[]>
        {
            { start, Array.Empty<(int, int)>() }
        };
        var coordsToCheck = paths.Keys.ToList();

        while(coordsToCheck.Any())
        {
            var coords = coordsToCheck.ToArray();
            coordsToCheck.Clear();
            foreach ((var x, var y) in coords)
            {
                foreach ((var nextX, var nextY) in graph.GetNextElevations((x, y)))
                {
                    var newPath = paths[(x,y)].Append((nextX, nextY)).ToArray();
                    if (!paths.ContainsKey((nextX,nextY)))
                    {
                        paths[(nextX, nextY)] = newPath;
                        coordsToCheck.Add((nextX,nextY));
                    }
                    else if (paths[(nextX, nextY)].Length > newPath.Length)
                    {
                        paths[(nextX, nextY)] = newPath;
                    }
                }
            }
        }

        return paths;
    }

    private static void Part1(string inputPath)
    {
        var graph = ReadAndParseInput(inputPath);

        var start = graph.SearchForStart();
        var exit = graph.SearchForExit();
        
        var paths = graph.FindPaths(start, exit);

        var result = paths.TryGetValue(exit, out var res) ? res.Length : 0;

        Console.WriteLine("Part1 result: " + result);
    }

    private static IEnumerable<(int, int)> SearchForAlternativeStarts(this int[][] graph)
    {
        for (int y = 0; y < graph.Length; y++)
        {
            for (int x = 0; x < graph[0].Length; x++)
            {
                if (graph[y][x] is 49)
                {
                    yield return (x,y);
                }
            }
        }
    }

    private static IEnumerable<(int, int)[]> GetPotentialHikingTrails(this int[][] graph)
    {
        var exit = graph.SearchForExit();

        foreach (var start in graph.SearchForAlternativeStarts())
        {
            var path = graph.FindPaths(start, exit);
            if (path.TryGetValue(exit, out var res))
            {
                yield return res;
            }
        }
    }

    private static void Part2(string inputPath)
    {
        var graph = ReadAndParseInput(inputPath);

        var starts = graph.SearchForAlternativeStarts();
        var exit = graph.SearchForExit();

        var trails = graph.GetPotentialHikingTrails();

        var result = trails.Min(t => t.Length);

        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
    
}
