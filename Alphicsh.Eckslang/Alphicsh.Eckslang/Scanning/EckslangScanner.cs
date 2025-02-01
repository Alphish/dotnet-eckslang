using System.Text.RegularExpressions;
using Alphicsh.Eckslang.Failures;

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

    public ReadOnlySpan<char> Head => Content.AsSpan(Position, Length - Position);
    public ReadOnlySpan<char> Tail => Content.AsSpan(0, Position);

    // -------------
    // Scanning past
    // -------------

    public bool HasAhead(char c)
        => CurrentCharacter == c;

    public bool HasAhead(string str)
        => Head.StartsWith(str);

    public bool HasAhead(string str, StringComparison comparison)
        => Head.StartsWith(str, comparison);

    public bool HasAhead(Regex regex)
    {
        var enumeration = regex.EnumerateMatches(Head);
        return enumeration.MoveNext() && enumeration.Current.Index == 0;
    }

    public bool TrySkip(char c)
    {
        if (CurrentCharacter != c)
            return false;

        Position++;
        return true;
    }

    public bool TrySkip(string str)
    {
        if (!Head.StartsWith(str))
            return false;

        Position += str.Length;
        return true;
    }

    public bool TrySkip(string str, StringComparison comparison)
    {
        if (!Head.StartsWith(str, comparison))
            return false;

        Position += str.Length;
        return true;
    }

    public bool TrySkip(Regex regex)
    {
        var enumeration = regex.EnumerateMatches(Head);
        if (!enumeration.MoveNext() || enumeration.Current.Index > 0)
            return false;

        Position += enumeration.Current.Length;
        return true;
    }

    public bool Expect(char c, EckslangFailureGenerator? failureGenerator = null)
        => TrySkip(c) || FailAndReturnFalse(failureGenerator ?? UnexpectedCharacterFailure.GeneratorFor(c));

    public bool Expect(string str, EckslangFailureGenerator? failureGenerator = null)
        => TrySkip(str) || FailAndReturnFalse(failureGenerator ?? UnexpectedStringFailure.GeneratorFor(str));

    public bool Expect(string str, StringComparison comparison, EckslangFailureGenerator? failureGenerator = null)
        => TrySkip(str, comparison) || FailAndReturnFalse(failureGenerator ?? UnexpectedStringFailure.GeneratorFor(str));

    public bool Expect(Regex regex, EckslangFailureGenerator? failureGenerator = null)
        => TrySkip(regex) || FailAndReturnFalse(failureGenerator ?? UnmatchedRegexFailure.GeneratorFor(regex));

    // -------
    // Peeking
    // -------

    public ReadOnlySpan<char> Peek(char c)
    {
        if (CurrentCharacter != c)
            return ReadOnlySpan<char>.Empty;

        return Head.Slice(0, 1);
    }

    public ReadOnlySpan<char> Peek(string str)
    {
        if (!Head.StartsWith(str))
            return ReadOnlySpan<char>.Empty;

        return Head.Slice(0, str.Length);
    }

    public ReadOnlySpan<char> Peek(string str, StringComparison comparison)
    {
        if (!Head.StartsWith(str, comparison))
            return ReadOnlySpan<char>.Empty;

        return Head.Slice(0, str.Length);
    }

    public ReadOnlySpan<char> Peek(Regex regex)
    {
        var enumeration = regex.EnumerateMatches(Head);
        if (!enumeration.MoveNext() || enumeration.Current.Index > 0)
            return ReadOnlySpan<char>.Empty;

        return Head.Slice(0, enumeration.Current.Length);
    }

    // -------
    // Reading
    // -------

    private ReadOnlySpan<char> TryAdvance(ReadOnlySpan<char> span)
    {
        Position += span.Length;
        return span;
    }

    public ReadOnlySpan<char> TryRead(char c)
        => TryAdvance(Peek(c));

    public ReadOnlySpan<char> TryRead(string str)
        => TryAdvance(Peek(str));

    public ReadOnlySpan<char> TryRead(string str, StringComparison comparison)
        => TryAdvance(Peek(str, comparison));

    public ReadOnlySpan<char> TryRead(Regex regex)
        => TryAdvance(Peek(regex));

    private ReadOnlySpan<char> AdvanceOrFail(ReadOnlySpan<char> span, EckslangFailureGenerator failureGenerator)
    {
        if (span.Length == 0)
            Fail(failureGenerator);

        Position += span.Length;
        return span;
    }

    public ReadOnlySpan<char> Read(char c, EckslangFailureGenerator? failureGenerator = null)
        => AdvanceOrFail(Peek(c), failureGenerator ?? UnexpectedCharacterFailure.GeneratorFor(c));

    public ReadOnlySpan<char> Read(string str, EckslangFailureGenerator? failureGenerator = null)
        => AdvanceOrFail(Peek(str), failureGenerator ?? UnexpectedStringFailure.GeneratorFor(str));

    public ReadOnlySpan<char> Read(string str, StringComparison comparison, EckslangFailureGenerator? failureGenerator = null)
        => AdvanceOrFail(Peek(str, comparison), failureGenerator ?? UnexpectedStringFailure.GeneratorFor(str));

    public ReadOnlySpan<char> Read(Regex regex, EckslangFailureGenerator? failureGenerator = null)
        => AdvanceOrFail(Peek(regex), failureGenerator ?? UnmatchedRegexFailure.GeneratorFor(regex));

    // ------------------------
    // Position/cursor handling
    // ------------------------

    private IEckslangCursor LastCursor { get; set; }

    public void SkipNext()
    {
        Position++;
    }

    public void JumpBy(int length)
    {
        Position += length;
    }

    public void JumpTo(int position)
    {
        Position = position;
    }

    public void JumpTo(IEckslangCursor cursor)
    {
        LastCursor = cursor;
        Position = cursor.Position;
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

    // --------
    // Failures
    // --------

    public IEckslangFailure? Failure { get; private set; }
    public bool IsFailed => Failure != null;

    public void Fail(IEckslangFailure failure)
    {
        Failure = failure;
        Position = Length; // go to the end, since there's no point in scanning anymore
        ScanFailed?.Invoke(this, new EckslangFailureEventArgs(failure));
    }

    public void Fail(EckslangFailureGenerator failureGenerator)
    {
        var failure = failureGenerator(this);
        Fail(failure);
    }

    private bool FailAndReturnFalse(EckslangFailureGenerator failureGenerator)
    {
        Fail(failureGenerator);
        return false;
    }

    public event EckslangFailureEventHandler? ScanFailed;
}
