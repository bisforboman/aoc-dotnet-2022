namespace CSharp;

public static class Program3
{
    private static int[][] ReadAndParseInput(string filePath) => 
        File.ReadAllLines(filePath)
            .Select(i => i
                .Select(j => j - '0')
                .ToArray())
            .ToArray();

    private static bool Left(this int[][] grid, int x, int y)
    {
        for (int i = x-1; i >= 0; i--)
        {
            if (grid[y][i] >= grid[y][x])
            {
                return false;
            }
        }

        return true;
    }

    private static bool Right(this int[][] grid, int x, int y)
    {
        var xMax = grid.Length;
        for (int i = x+1; i < xMax; i++)
        {
            if (grid[y][i] >= grid[y][x])
            {
                return false;
            }
        }

        return true;
    }

    private static bool Above(this int[][] grid, int x, int y)
    {
        for (int i = y-1; i >= 0; i--)
        {
            if (grid[i][x] >= grid[y][x])
            {
                return false;
            }
        }

        return true;
    }

    private static bool Below(this int[][] grid, int x, int y)
    {
        var yMax = grid[0].Length;
        for (int i = y+1; i < yMax; i++)
        {
            if (grid[i][x] >= grid[y][x])
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsVisible(this int[][] grid, int x, int y) => 
        grid.Left(x, y) || 
        grid.Right(x, y) || 
        grid.Above(x, y) || 
        grid.Below(x, y);

    private static bool[][] Traverse(int[][] grid)
    {
        var xMax = grid.Length;
        var yMax = grid[0].Length;

        var ret = new bool[yMax][];

        for (int y = 0; y < yMax; y++)
        {
            ret[y] = new bool[xMax];
            for (int x = 0; x < xMax; x++)
            {
                ret[y][x] = grid.IsVisible(x, y);
            }
        }

        return ret;
    }

    private static void Part1(string inputPath)
    {
        var inputs = ReadAndParseInput(inputPath);

        var res = Traverse(inputs);

        var result = res.SelectMany(e => e).Where(e => e).Count();

        Console.WriteLine("Part1 result: " + result);
    }

    private static int AmountOfTreesLeftOf(this int[][] grid, int x, int y)
    {
        var acc = 0;
        for (int i = x-1; i >= 0; i--)
        {
            acc++;
            if (grid[y][i] >= grid[y][x])
            {
                break;
            }
        }

        return acc;
    }

    private static int AmountOfTreesRightOf(this int[][] grid, int x, int y)
    {
        var acc = 0;
        var xMax = grid.Length;
        for (int i = x+1; i < xMax; i++)
        {
            acc++;
            if (grid[y][i] >= grid[y][x])
            {
                break;
            }
        }

        return acc;
    }

    private static int AmountOfTreesAbove(this int[][] grid, int x, int y)
    {
        var acc = 0;
        for (int i = y-1; i >= 0; i--)
        {
            acc++;
            if (grid[i][x] >= grid[y][x])
            {
                break;
            }
        }

        return acc;
    }

    private static int AmountOfTreesBelow(this int[][] grid, int x, int y)
    {
        var acc = 0;
        var yMax = grid[0].Length;
        for (int i = y+1; i < yMax; i++)
        {
            acc++;
            if (grid[i][x] >= grid[y][x])
            {
                break;
            }
        }

        return acc;
    }

    private static int AmountOfTrees(this int[][] grid, int x, int y) => 
        grid.AmountOfTreesLeftOf(x, y) *
        grid.AmountOfTreesRightOf(x, y) * 
        grid.AmountOfTreesAbove(x, y) *
        grid.AmountOfTreesBelow(x, y);

    private static int[][] Traverse2(int[][] grid)
    {
        var xMax = grid.Length;
        var yMax = grid[0].Length;

        var ret = new int[yMax][];

        for (int y = 0; y < yMax; y++)
        {
            ret[y] = new int[xMax];
            for (int x = 0; x < xMax; x++)
            {
                ret[y][x] = grid.AmountOfTrees(x, y);
            }
        }

        return ret;
    }

    private static void Part2(string inputPath)
    {
        var inputs = ReadAndParseInput(inputPath);

        var result = Traverse2(inputs).SelectMany(e => e).Max();

        Console.WriteLine("Part2 result: " + result);
    }

    public static void Main(string[] args)
    {
        Part1(args[0]);
        Part2(args[0]);
    }
    
}
