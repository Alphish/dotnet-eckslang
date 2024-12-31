
using System.Text.RegularExpressions;

namespace Alphicsh.Eckslang.Scanning;

public class EckslangCursor : IEckslangCursor
{
    public int Position { get; private set; }
    public int Line { get; private set; }
    public int Column { get; private set; }

    public EckslangCursor()
    {
        Position = 0;
        Line = 1;
        Column = 1;
    }

    public EckslangCursor(int position, int line, int column)
    {
        Position = position;
        Line = line;
        Column = column;
    }

    private static Regex NewlinePattern { get; } = new Regex("(\r\n|\r|\n)", RegexOptions.Compiled);

    public void Advance(ReadOnlySpan<char> span, IEckslangScanner scanner)
    {
        var addedLength = span.Length;
        Position += addedLength;

        if (span[^1] == '\r' && scanner.Head.Length > 0 && scanner.Head[0] == '\n')
            span = span.Slice(0, span.Length - 1);

        var addedLines = NewlinePattern.Count(span);
        if (addedLines == 0)
        {
            Column += addedLength;
            return;
        }
        else
        {
            Line += addedLines;
            Column = addedLength - span.LastIndexOfAny('\r', '\n');
        }
    }

    public void Backtrack(ReadOnlySpan<char> span, IEckslangScanner scanner)
    {
        var backLength = span.Length;
        Position -= backLength;
        if (Column > backLength)
        {
            Column -= backLength;
            return;
        }

        if (span[^1] == '\r' && Column > 1)
            span = span.Slice(0, span.Length - 1);

        var backLines = NewlinePattern.Count(span);
        Line -= backLines;

        var tail = scanner.Tail;
        if (span[0] == '\n' && tail.Length > 0 && tail[^1] == '\r')
            tail = tail.Slice(0, tail.Length - 1);

        Column = Position - tail.LastIndexOfAny('\r', '\n');
    }

    public IEckslangCursor Clone()
    {
        return new EckslangCursor(Position, Line, Column);
    }

    public void Assign(IEckslangCursor cursor)
    {
        Position = cursor.Position;
        Line = cursor.Line;
        Column = cursor.Column;
    }
}
