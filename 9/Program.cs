namespace CSharp;

public static class Program3
{
    private static string[] ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath);

    private static (int, int) TranslateInstruction((string Direction, int Amount) Tuple) => Tuple.Direction switch
    {
        "R" => (Tuple.Amount, 0),
        "L" => (-Tuple.Amount, 0),
        "U" => (0, -Tuple.Amount),
        "D" => (0, Tuple.Amount),
        _ => throw new Exception("not good")
    };

    private static IEnumerable<(int,int)> GetMovements((int X, int Y) delta, (int X, int Y) head)
    {
        // move down
        for (int y = 1; y <= delta.Y; y++)
        {
            yield return (head.X, head.Y + y);
        }

        // move up
        for (int y = -1; y >= delta.Y; y--)
        {
            yield return (head.X, head.Y + y);
        }

        // move right
        for (int x = 1; x <= delta.X; x++)
        {
            yield return (head.X + x, head.Y);
        }

        // move left
        for (int x = -1; x >= delta.X; x--)
        {
            yield return (head.X + x, head.Y);
        }
    }

    private static bool IsntTouching((int X, int Y) a, (int X, int Y) b) => Math.Abs(a.Y - b.Y) > 1 || Math.Abs(a.X - b.X) > 1;

    private static ((int, int), (int, int)) Traverse(this int[,] board, (int X, int Y) delta, (int X, int Y) head, (int X, int Y) tail)
    {
        var movements = GetMovements(delta, head).ToArray();

        for (int i = 0; i < movements.Length; i++)
        {
            if (IsntTouching(movements[i], tail))
            {
                tail = head;
            }

            head = movements[i];

            board[tail.X, tail.Y] = 1;
        }

        return (head, tail);
    }

    private static (string,int) ToTuples(string s)
    {
        var ret = s.Split(' ');
        return (ret[0], int.Parse(ret[1]));
    }

    private static void Part1(string inputPath)
    {
        var inputs = ReadAndParseInput(inputPath)
            .Select(ToTuples)
            .Select(TranslateInstruction);

        var board = new int[1000,1000];
        var head = (board.GetLength(0) / 2, board.GetLength(1) / 2);
        var tail = head;

        foreach (var xy in inputs)
        {
            (head, tail) = board.Traverse(xy, head, tail);
        }

        // for (var y = 0; y < board.GetLength(1); y++)
        // {
        //     for (var x = 0; x < board.GetLength(0); x++)
        //     {
        //         Console.Write($"({x},{y}) ");
        //     }
        //     Console.WriteLine();
        // }

        // Console.WriteLine("-- --");

        // for (var y = 0; y < board.GetLength(1); y++)
        // {
        //     for (var x = 0; x < board.GetLength(0); x++)
        //     {
        //         Console.Write($"{(board[x, y] > 0 ? '#' : '.')}");
        //     }
        //     Console.WriteLine();
        // }

        var result = 0;
        for (var y = 0; y < board.GetLength(1); y++)
        {
            for (var x = 0; x < board.GetLength(0); x++)
            {
                result += board[x,y];
            }
        }

        Console.WriteLine("Part1 result: " + result);
    }

    private static void Part2(string inputPath)
    {
        var inputs = ReadAndParseInput(inputPath);

        var result = "";

        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
    
}
