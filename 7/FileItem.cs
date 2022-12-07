namespace CSharp;

public class FileItem
{
    public string Name { get; } = string.Empty;
    public long Size { get; }
    public DirItem? Parent { get; }

    public FileItem(string name, long size, DirItem parent)
    {
        Name = name;
        Size = size;
        Parent = parent;
    }

    public long GetSize() => Size;
    public override string ToString() => $"[File: {Name} - {Size}]";
}
