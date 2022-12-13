using System.Text.Json;

namespace CSharp;

public static class Program
{
    private static string[] ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath)
            .ToArray();

    private static IEnumerable<(JsonElement, JsonElement)> ParseToPairs(this string[] ss)
    {
        for (int i = 0; i < ss.Length; i += 3)
        {
            var leftIndex = i;
            var rightIndex = leftIndex + 1;

            yield return (
                JsonDocument.Parse(ss[leftIndex]).RootElement, 
                JsonDocument.Parse(ss[rightIndex]).RootElement
            );
        }
    }

    private static JsonElement ToArray(this JsonElement ele) => 
        JsonDocument.Parse($"[{ele.GetInt32()}]").RootElement;

    private static int Compare(JsonElement left, JsonElement right)
    {
        // Console.WriteLine($"COMPARE: {left} vs {right}");
        if (left.ValueKind is JsonValueKind.Number && right.ValueKind is JsonValueKind.Number)
        {
            return left.GetInt32() - right.GetInt32();
        }

        if (left.ValueKind is JsonValueKind.Array && right.ValueKind is JsonValueKind.Number)
        {
            return Compare(left, right.ToArray());
        }

        if (left.ValueKind is JsonValueKind.Number && right.ValueKind is JsonValueKind.Array)
        {
            return Compare(left.ToArray(), right);
        }

        if (left.ValueKind is JsonValueKind.Array && right.ValueKind is JsonValueKind.Array)
        {
            var zipped = Enumerable.Zip(left.EnumerateArray(), right.EnumerateArray());
            
            foreach ((var innerLeft, var innerRight) in zipped)
            {
                var compare = Compare(innerLeft, innerRight);
                if (compare is 0)
                {
                    continue;
                }
                else
                {
                    return compare;
                }
            }

            return left.GetArrayLength() - right.GetArrayLength();
        }

        throw new Exception("not good!");
    }

    private static void Part1(string inputPath)
    {
        var input = ReadAndParseInput(inputPath)
            .ParseToPairs()
            .Select((p,i) => (Pair: p, Index: i+1))
            .ToArray();

        // foreach (((var left, var right), var index) in input)
        // {
        //     Console.WriteLine($"{index} | {Compare(left, right),2} | {left} vs {right}");
        // }

        var result = input
            .Where(pi => Compare(pi.Pair.Item1, pi.Pair.Item2) < 0)
            .Sum(pi => pi.Index);

        Console.WriteLine("Part1 result: " + result);
    }

    private static void Part2(string inputPath)
    {
        var divider1Json = JsonDocument.Parse("[[2]]").RootElement;
        var divider2Json = JsonDocument.Parse("[[6]]").RootElement;

        var input = ReadAndParseInput(inputPath)
            .ParseToPairs()
            .SelectMany(pair => new JsonElement[] { pair.Item1, pair.Item2 })
            .Append(divider1Json)
            .Append(divider2Json)
            .OrderBy(p => p, Comparer<JsonElement>.Create(Compare))
            .Select((p,i) => (Pair: p, Index: i+1))
            .ToList();

        var divider1 = input.Find(pi => Compare(pi.Pair, divider1Json) == 0);
        var divider2 = input.Find(pi => Compare(pi.Pair, divider2Json) == 0);

        var result = divider1.Index * divider2.Index;

        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
    
}
