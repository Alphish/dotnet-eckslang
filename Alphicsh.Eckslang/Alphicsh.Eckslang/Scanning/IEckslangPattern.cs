namespace Alphicsh.Eckslang.Scanning;

public interface IEckslangPattern
{
    ReadOnlySpan<char> Match(IEckslangScanner scanner);
}
