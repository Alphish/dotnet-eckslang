namespace Alphicsh.Eckslang.Scanning;

public interface IEckslangCursor
{
    int Position { get; }
    int Line { get; }
    int Column { get; }

    void Advance(ReadOnlySpan<char> span, IEckslangScanner scanner);
    void Backtrack(ReadOnlySpan<char> span, IEckslangScanner scanner);

    IEckslangCursor Clone();
    void Assign(IEckslangCursor cursor);
}
