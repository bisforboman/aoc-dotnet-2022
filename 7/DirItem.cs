using System.Collections;

namespace CSharp;

public class DirItem : IItem, IEnumerable<DirItem>
{
    public string Name { get; } = string.Empty;
    private long Size { get; set; } = -1;
    public DirItem? Parent { get; }
    public Dictionary<string, DirItem> Directories = new();
    public List<FileItem> Files = new();

    public DirItem(string name, DirItem? parent)
    {
        Name = name;
        Parent = parent;
    }

    private void UpdateSize()
    {
        long acc = Files.Sum(f => f.GetSize());

        foreach (var f in Directories.Values)
        {
            acc += f.GetSize();
        }

        Size = acc;
    }

    public long GetSize()
    {
        if (Size > 0)
        {
            return Size;
        }

        UpdateSize();
        return Size;
    }

    public override string ToString() => $"[Dir: {Name}]";

    public IEnumerator<DirItem> GetEnumerator() => Recursive(this).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private static IEnumerable<DirItem> Recursive(DirItem dirItem)
    {
        yield return dirItem;

        foreach (var d in dirItem.Directories.Values.SelectMany(Recursive))
        {
            yield return d;
        }
    }

}
