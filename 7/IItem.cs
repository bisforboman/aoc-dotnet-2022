namespace CSharp;

public interface IItem
{
    public string Name { get; }
    public DirItem? Parent { get; }
    public long GetSize();
}
