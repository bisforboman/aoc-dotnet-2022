namespace CSharp;

public class Monkey
{
    public Monkey(
        int index,
        Func<UInt128, UInt128> operation,
        UInt128 isDivisibleBy,
        (int, int) throwsTo,
        UInt128[] items)
    {
        Index = index;
        Items = items.ToList();
        Operation = operation;
        IsDivisibleBy = isDivisibleBy;
        ThrowsTo = throwsTo;
    }

    public int Index { get; }
    public List<UInt128> Items { get; } = new();
    public Func<UInt128, UInt128> Operation { get; }
    public UInt128 IsDivisibleBy { get; }
    public UInt128 WorryLevelReducer { get; set; } = UInt128.One;
    public UInt128 Inspects { get; private set; }
    public (int True, int False) ThrowsTo { get; }

    public IEnumerable<(int, UInt128)> Throw()
    {
        Inspects += (UInt128)Items.Count;

        foreach (var item in Items)
        {
            var newValue = Operation.Invoke(item) / WorryLevelReducer;

            if (newValue % IsDivisibleBy == 0)
            {
                yield return (ThrowsTo.True, newValue);
            }
            else
            {
                yield return (ThrowsTo.False, newValue);
            }
        }

        Items.Clear();
    }

    public static Monkey WithWorryLevelReducer(Monkey m, UInt128 w)
    {
        m.WorryLevelReducer = w;
        return m;
    }

    public override string ToString() => $"Monkey {Index}: {string.Join(",", Items)}";

    private static Func<UInt128, UInt128> ParseOperation(string s)
    {
        var operatee = s.Split(' ')[1];
        if (s.StartsWith("+"))
        {
            return x => x + UInt128.Parse(operatee);
        }

        if (s.StartsWith("*"))
        {
            if (s.EndsWith("old"))
            {
                return x => x * x;
            }

            return x => x * UInt128.Parse(operatee);
        }

        throw new Exception("Unknown operation! " + s);
    }

    public static Monkey Create(string[] ss)
    {
        var index = ss[0][7] - '0';
        var items = ss[1]
            .Split(new[] { ':', ',' })
            .Select(s => s.Trim())
            .Skip(1)
            .Select(UInt128.Parse);

        var operation = ParseOperation(ss[2].Split("old", 2)[1].Trim());
            
        var isDivisibleBy = UInt128.Parse(ss[3].Trim().Split(' ').Last());

        var ifTrue = int.Parse(ss[4].Split(' ').Last());
        var ifFalse = int.Parse(ss[5].Split(' ').Last());

        return new(
            index: index,
            operation: operation,
            isDivisibleBy: isDivisibleBy,
            throwsTo: (ifTrue, ifFalse),
            items: items.ToArray()
        );
    }
}
