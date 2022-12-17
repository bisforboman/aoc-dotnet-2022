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

    private static void ShortestPaths(this Dictionary<Valve, Valve[]> dict, Dictionary<Valve, IEnumerable<Valve>> paths, Valve from)
    {
        var newSteps = new List<Valve>();

        foreach (var v in paths[from].Where(p => !dict.ContainsKey(p)))
        {
            newSteps.Add(v);
        }

        foreach (var v in newSteps.Where(p => !dict.ContainsKey(p)))
        {
            dict.Add(v, dict[from].Append(v).ToArray());
            dict.ShortestPaths(paths, v);
        }
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

    private static int GetRate(int time, KeyValuePair<Valve, Valve[]> v, Dictionary<string, int> rates) => 
        (time - v.Value.Length - 1) * rates[v.Key.Name];

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

        var from = new Valve("AA", 0);

        var bestWayFromEachPoint = new Dictionary<Valve, Dictionary<Valve, Valve[]>>();
        
        foreach (var v in pathsDict.Keys)
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

        var allOrders = AllOrderings(valvesToVisit, Array.Empty<Valve>());

        foreach (var o in allOrders)
        {
            for (int time = 30; time > 29; time--)
            {
                
            }
        }

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
