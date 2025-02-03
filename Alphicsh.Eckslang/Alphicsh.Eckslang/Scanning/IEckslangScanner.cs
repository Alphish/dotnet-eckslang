using System.Text.RegularExpressions;
using Alphicsh.Eckslang.Failures;

namespace Alphicsh.Eckslang.Scanning;

public interface IEckslangScanner
{
    int Length { get; }
    int Position { get; }
    char CurrentCharacter { get; }
    bool EndOfContent { get; }

    ReadOnlySpan<char> Head { get; }
    ReadOnlySpan<char> Tail { get; }

    IEckslangCursor Cursor { get; }
    void SkipNext();
    void MoveBy(int length);
    void MoveTo(int position);
    void MoveTo(IEckslangCursor cursor);

    bool HasAhead(char c);
    bool HasAhead(string str);
    bool HasAhead(string str, StringComparison comparison);
    bool HasAhead(Regex regex);

    bool TrySkip(char c);
    bool TrySkip(string str);
    bool TrySkip(string str, StringComparison comparison);
    bool TrySkip(Regex regex);

    bool Expect(char c, EckslangFailureGenerator? failureGenerator = null);
    bool Expect(string str, EckslangFailureGenerator? failureGenerator = null);
    bool Expect(string str, StringComparison comparison, EckslangFailureGenerator? failureGenerator = null);
    bool Expect(Regex regex, EckslangFailureGenerator? failureGenerator = null);

    ReadOnlySpan<char> Peek(char c);
    ReadOnlySpan<char> Peek(string str);
    ReadOnlySpan<char> Peek(string str, StringComparison comparison);
    ReadOnlySpan<char> Peek(Regex regex);

    ReadOnlySpan<char> TryRead(char c);
    ReadOnlySpan<char> TryRead(string str);
    ReadOnlySpan<char> TryRead(string str, StringComparison comparison);
    ReadOnlySpan<char> TryRead(Regex regex);

    ReadOnlySpan<char> Read(char c, EckslangFailureGenerator? failureGenerator = null);
    ReadOnlySpan<char> Read(string str, EckslangFailureGenerator? failureGenerator = null);
    ReadOnlySpan<char> Read(string str, StringComparison comparison, EckslangFailureGenerator? failureGenerator = null);
    ReadOnlySpan<char> Read(Regex regex, EckslangFailureGenerator? failureGenerator = null);

    IEckslangFailure? Failure { get; }
    bool IsFailed { get; }
    void Fail(IEckslangFailure failure);
    void Fail(EckslangFailureGenerator failureGenerator);
    event EckslangFailureEventHandler? ScanFailed;
}
