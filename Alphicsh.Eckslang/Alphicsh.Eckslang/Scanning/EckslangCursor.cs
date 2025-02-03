
using System.Text.RegularExpressions;

namespace Alphicsh.Eckslang.Scanning;

public struct EckslangCursor : IEckslangCursor
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

    public override string ToString()
    {
        return $"Ln: {Line}, Col: {Column}, Pos: {Position}";
    }

    private static Regex NewlinePattern { get; } = new Regex("(\r\n|\r|\n)", RegexOptions.Compiled);

    public IEckslangCursor Advance(ReadOnlySpan<char> span, IEckslangScanner scanner)
    {
        var addedLength = span.Length;
        if (span[^1] == '\r' && scanner.Head.Length > 0 && scanner.Head[0] == '\n')
            span = span.Slice(0, span.Length - 1);

        var addedLines = NewlinePattern.Count(span);

        var newPosition = Position + addedLength;
        var newLine = Line + addedLines;
        var newColumn = addedLines == 0 ? Column + addedLength : addedLength - span.LastIndexOfAny('\r', '\n');
        return new EckslangCursor(newPosition, newLine, newColumn);
    }

    public IEckslangCursor Backtrack(ReadOnlySpan<char> span, IEckslangScanner scanner)
    {
        var backLength = span.Length;
        var newPosition = Position - backLength;
        if (Column > backLength)
            return new EckslangCursor(newPosition, Line, Column - backLength);

        if (span[^1] == '\r' && Column > 1)
            span = span.Slice(0, span.Length - 1);

        var tail = scanner.Tail;
        if (span[0] == '\n' && tail.Length > 0 && tail[^1] == '\r')
            tail = tail.Slice(0, tail.Length - 1);

        var backLines = NewlinePattern.Count(span);
        var newLine = Line - backLines;
        var newColumn = newPosition - tail.LastIndexOfAny('\r', '\n');
        return new EckslangCursor(newPosition, newLine, newColumn);
    }
}
