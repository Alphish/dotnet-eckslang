namespace Alphicsh.Eckslang.Scanning;

public interface IEckslangScanner
{
    int Position { get; }
    ReadOnlySpan<char> Head { get; }
    ReadOnlySpan<char> ReadSpan(IEckslangPattern pattern);
}
