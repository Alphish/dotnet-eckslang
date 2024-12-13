namespace Alphicsh.Eckslang.Scanning;

public interface IEckslangScanner
{
    ReadOnlySpan<char> Head { get; }
    ReadOnlySpan<char> ReadSpan(IEckslangPattern pattern);
}
