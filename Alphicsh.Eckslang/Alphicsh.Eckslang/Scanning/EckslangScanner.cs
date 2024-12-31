namespace Alphicsh.Eckslang.Scanning;

public class EckslangScanner : IEckslangScanner
{
    public string Content { get; }
    public int Position { get; private set; }
    public IEckslangCursor Cursor { get; }

    public EckslangScanner(string content)
    {
        Content = content;
        Position = 0;
        Cursor = new EckslangCursor();
    }

    public ReadOnlySpan<char> Head => Content.AsSpan(Position);
    public ReadOnlySpan<char> Tail => Content.AsSpan(0, Position);

    public ReadOnlySpan<char> ReadSpan(IEckslangPattern pattern)
    {
        var match = pattern.Match(this);
        if (match.Length == 0)
            return match;

        Position += match.Length;
        Cursor.Advance(match, this);
        return match;
    }

    public void JumpTo(IEckslangCursor cursor)
    {
        Position = cursor.Position;
        Cursor.Assign(cursor);
    }

    public void JumpTo(int position)
    {
        var addedLength = position - Position;
        if (addedLength > 0)
        {
            var advanceSpan = Content.AsSpan(Position, addedLength);
            Position = position;
            Cursor.Advance(advanceSpan, this);
        }
        else if (addedLength < 0)
        {
            var backSpan = Content.AsSpan(position, -addedLength);
            Position = position;
            Cursor.Backtrack(backSpan, this);
        }
    }
}
