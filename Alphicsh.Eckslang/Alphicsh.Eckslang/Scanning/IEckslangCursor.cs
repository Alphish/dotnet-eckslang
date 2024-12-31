namespace Alphicsh.Eckslang.Scanning;

public interface IEckslangCursor
{
    int Position { get; }
    int Line { get; }
    int Column { get; }

    IEckslangCursor Advance(ReadOnlySpan<char> span, IEckslangScanner scanner);
    IEckslangCursor Backtrack(ReadOnlySpan<char> span, IEckslangScanner scanner);
}
