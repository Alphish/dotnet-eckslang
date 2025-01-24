using System.Text.RegularExpressions;

namespace Alphicsh.Eckslang.Scanning;

public class EckslangScanner : IEckslangScanner
{
    public string Content { get; }
    public int Length { get; }
    public int Position { get; private set; }
    public char CurrentCharacter => Content[Position];
    public bool EndOfContent => Position >= Length;

    public IEckslangCursor Cursor => UpdateCursor();

    public EckslangScanner(string content)
    {
        Length = content.Length;
        Content = content + '\0';
        Position = 0;
        LastCursor = new EckslangCursor();
    }

    public ReadOnlySpan<char> Head => Content.AsSpan(Position);
    public ReadOnlySpan<char> Tail => Content.AsSpan(0, Position);

    public void SkipChar()
    {
        Position++;
    }

    public void ExpectChar(char c)
    {
        if (Content[Position] != c)
            throw new FormatException($"Unexpected character '{Content[Position]}', '{c}' expected.");

        Position++;
    }

    public bool TrySkipChar(char c)
    {
        if (Content[Position] != c)
            return false;

        Position++;
        return true;
    }

    public void ExpectRegex(Regex regex)
    {
        var enumeration = regex.EnumerateMatches(Content, Position);
        if (!enumeration.MoveNext())
            throw new FormatException($"Could not read expected pattern.");

        Position += enumeration.Current.Length;
    }

    public void SkipRegex(Regex regex)
    {
        var enumeration = regex.EnumerateMatches(Content, Position);
        enumeration.MoveNext();
        Position += enumeration.Current.Length;
    }

    public bool TrySkipRegex(Regex regex)
    {
        var enumeration = regex.EnumerateMatches(Content, Position);
        if (!enumeration.MoveNext())
            return false;

        Position += enumeration.Current.Length;
        return true;
    }

    public ReadOnlySpan<char> TryReadRegex(Regex regex)
    {
        var enumeration = regex.EnumerateMatches(Content, Position);
        if (!enumeration.MoveNext())
            return ReadOnlySpan<char>.Empty;

        var result = Head.Slice(0, enumeration.Current.Length);
        Position += enumeration.Current.Length;
        return result;
    }

    public ReadOnlySpan<char> ReadRegex(Regex regex)
    {
        var enumeration = regex.EnumerateMatches(Content, Position);
        if (!enumeration.MoveNext())
            throw new FormatException($"Could not read expected pattern at {Position}.");

        var result = Head.Slice(0, enumeration.Current.Length);
        Position += enumeration.Current.Length;
        return result;
    }

    public ReadOnlySpan<char> ReadPattern(IEckslangPattern pattern)
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
