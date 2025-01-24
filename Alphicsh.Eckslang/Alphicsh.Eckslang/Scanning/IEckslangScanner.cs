using System.Text.RegularExpressions;

namespace Alphicsh.Eckslang.Scanning;

public interface IEckslangScanner
{
    int Position { get; }
    char CurrentCharacter { get; }
    bool EndOfContent { get; }

    ReadOnlySpan<char> Head { get; }
    ReadOnlySpan<char> Tail { get; }

    IEckslangCursor Cursor { get; }

    void SkipChar();
    void ExpectChar(char c);
    bool TrySkipChar(char c);

    void ExpectRegex(Regex regex);
    void SkipRegex(Regex regex);
    bool TrySkipRegex(Regex regex);
    ReadOnlySpan<char> TryReadRegex(Regex regex);
    ReadOnlySpan<char> ReadRegex(Regex regex);

    ReadOnlySpan<char> ReadPattern(IEckslangPattern pattern);

    void JumpTo(IEckslangCursor cursor);
    void JumpTo(int position);
}
