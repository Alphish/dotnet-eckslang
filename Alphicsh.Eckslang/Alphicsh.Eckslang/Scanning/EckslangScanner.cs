namespace Alphicsh.Eckslang.Scanning;

public class EckslangScanner : IEckslangScanner
{
    public string Content { get; }
    public int Position { get; private set; }
    public IEckslangCursor Cursor => UpdateCursor();

    public EckslangScanner(string content)
    {
        Content = content;
        Position = 0;
        LastCursor = new EckslangCursor();
    }

    public ReadOnlySpan<char> Head => Content.AsSpan(Position);
    public ReadOnlySpan<char> Tail => Content.AsSpan(0, Position);

    public ReadOnlySpan<char> ReadSpan(IEckslangPattern pattern)
    {
        var match = pattern.Match(this);
        if (match.Length == 0)
            return match;

        Position += match.Length;
        return match;
    }

    // ------------------------
    // Position/cursor handling
    // ------------------------

    private IEckslangCursor LastCursor { get; set; }

    public void JumpTo(IEckslangCursor cursor)
    {
        LastCursor = cursor;
        Position = cursor.Position;
    }

    public void JumpTo(int position)
    {
        Position = position;
    }

    private IEckslangCursor UpdateCursor()
    {
        var positionChange = Position - LastCursor.Position;
        if (positionChange > 0)
        {
            var advanceSpan = Content.AsSpan(LastCursor.Position, positionChange);
            LastCursor = LastCursor.Advance(advanceSpan, this);
        }
        else if (positionChange < 0)
        {
            var backSpan = Content.AsSpan(Position, -positionChange);
            LastCursor = LastCursor.Backtrack(backSpan, this);
        }
        return LastCursor;
    }
}
