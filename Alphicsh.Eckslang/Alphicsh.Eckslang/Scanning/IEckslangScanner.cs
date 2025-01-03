namespace Alphicsh.Eckslang.Scanning;

public interface IEckslangScanner
{
    int Position { get; }
    IEckslangCursor Cursor { get; }
    bool EndOfContent { get; }

    ReadOnlySpan<char> Head { get; }
    ReadOnlySpan<char> Tail { get; }

    ReadOnlySpan<char> ReadSpan(IEckslangPattern pattern);

    void JumpTo(IEckslangCursor cursor);
    void JumpTo(int position);
}
