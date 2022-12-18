namespace CSharp;

public static class Program
{
    private record Valve(string Name, int Rate)
    {
        public override string ToString() => Name;
    };

    private static string[] ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath);

    private static Comparer<Valve> Comparer = Comparer<Valve>.Create((a, b) => b.Rate - a.Rate);

    // fill each dict-entry with allnodes
    private static (Dictionary<Valve, Valve[]>, IEnumerable<Valve>) ShortestPaths(
        Dictionary<Valve, Valve[]> pathToNode, 
        Dictionary<Valve, IEnumerable<Valve>> neighborsDict,
        HashSet<Valve> border,
        HashSet<Valve> visisted)
    {

        if (!border.Any())
        {
            return (pathToNode, Enumerable.Empty<Valve>());
        }

        var nextPossibleSteps = new List<(Valve,Valve)>();
        foreach (var nn in border)
        {
            var excludeVisited = neighborsDict[nn]
                .Where(b => !visisted.Contains(b))
                .Select(b => (nn,b));

            nextPossibleSteps.AddRange(excludeVisited);
        }

        foreach ((var start, var dest) in nextPossibleSteps)
        {
            var toReachStep = pathToNode[start].Append(dest).ToArray();
            if (toReachStep.Length < pathToNode[dest].Length)
            {
                pathToNode[dest] = toReachStep;
            }
        }

        foreach ((var start, var dest) in nextPossibleSteps)
        {
            ShortestPaths(pathToNode, neighborsDict, border, visisted.Append(dest).ToHashSet());
        }



        // foreach ((var v, var path) in list)
        // {
        //     dict.ShortestPaths(paths, v);
        // }
    }

    private static void Print(this Dictionary<Valve, Valve[]> paths, Valve from)
    {
        foreach (var p in paths)
        {
            Console.WriteLine($"To reach {p.Key.Name} from {from.Name}, go through {string.Join(" -> ", p.Value.Select(v => v.Name))}");
        }
    }

    private static (Valve, string[]) CreateValves(string s)
    {
        var c = s
            .Replace("Valve ", string.Empty)
            .Replace(" has flow rate", string.Empty)
            .Replace(" tunnels lead to valves ", string.Empty)
            .Replace(" tunnel leads to valve ", string.Empty)
            .Split('=', ';');

        return (new Valve(c[0], int.Parse(c[1])), c[2].Split(", "));
    }

    private static List<Valve[]> AllOrderings(Valve[] valvesToVisit, Valve[] visited)
    {
        var valvesLeft = valvesToVisit
            .Where(v => !visited.Contains(v))
            .ToArray();

        var list = new List<Valve[]>();

        if (!valvesLeft.Any())
        {
            list.Add(visited);
        }
        else
        {
            foreach (var v in valvesLeft)
            {
                var visit = visited.Append(v).ToArray();
                var pathsLeft = AllOrderings(valvesToVisit, visit);

                list.AddRange(pathsLeft);
            }
        }

        return list;
    }

    private static void Part1(string inputPath)
    {
        var input = ReadAndParseInput(inputPath);

        var valvesAndPaths = input.Select(CreateValves);

        var valves = valvesAndPaths.Select(v => v.Item1);

        var valvesDict = valvesAndPaths
            .ToDictionary(v => v.Item1.Name, v => v.Item1.Rate);

        var pathsDict = valvesAndPaths
            .ToDictionary(v => v.Item1, v => v.Item2.Select(v2 => new Valve(v2, valvesDict[v2])));

        var fromValve = new Valve("AA", 0);

        var bestWayFromEachPoint = new Dictionary<Valve, Dictionary<Valve, Valve[]>>();
        
        var bb = new Valve("BB", 13);

        foreach (var v in pathsDict.Keys.Skip(1).Take(1))
        {
            var dict = new Dictionary<Valve, Valve[]>
            {
                { v, Array.Empty<Valve>() }
            };

            dict.ShortestPaths(pathsDict, v);
            bestWayFromEachPoint[v] = dict;
        }
        
        var valvesToVisit = pathsDict.Keys
            .Where(p => p.Rate > 0)
            .ToArray();

        // var visited = new Valve[] { fromValve };
        // var allOrders = AllOrderings(valvesToVisit, visited)
        //     .ToArray();

        foreach (var b in bestWayFromEachPoint)
        {
            foreach (var p in b.Value)
            {
                Console.WriteLine($"{p.Key} - {string.Join(",", p.Value.AsEnumerable())}");
            }
        }

        // var allOrders = new Valve[1][]
        // {
        //     new Valve[] 
        //     {
        //         new Valve("AA", 0),
        //         new Valve("DD", 20),
        //         new Valve("BB", 13),
        //         new Valve("JJ", 21),
        //         new Valve("HH", 22),
        //         new Valve("EE", 3),
        //         new Valve("CC", 2),
        //     }
        // };

        // var list = new List<(int, Valve[])>();
        // foreach (var o in allOrders)
        // {
        //     var openValveAt = new List<(int, Valve)>();
        //     var time = 30;
        //     for (int v = 0; v < o.Length - 1 ; v++)
        //     {
        //         var current = o[v];
        //         var next = o[v+1];
        //         var steps = bestWayFromEachPoint[current][next];

        //         time -= steps.Length;
        //         time--;

        //         Console.WriteLine($"Going from {current} to {next} takes {steps.Length} (Path: {string.Join(",", steps.AsEnumerable())})");
        //         Console.WriteLine($"Time's at {(30 - time)}. Rate {next.Rate}. Total {time} * {next.Rate} = {time*next.Rate}");
        //         Console.WriteLine();

        //         openValveAt.Add((time, next));
        //     }

        //     var valvesOrder = openValveAt.Select(v => v.Item2).ToArray();
        //     var rates = openValveAt.Sum(v => v.Item2.Rate * v.Item1);

        //     list.Add((rates, valvesOrder));
        // }

        // foreach ((var rate, var path) in list.OrderByDescending(o => o.Item1).Take(1))
        // {
        //     Console.WriteLine($"Rate {rate} | {string.Join(",", path.AsEnumerable())}");
        // }

        var result = "";

        Console.WriteLine("Part1 result: " + result);
    }

    private static void Part2(string inputPath)
    {
        var input = ReadAndParseInput(inputPath);

        var result = "";

        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
    
}
