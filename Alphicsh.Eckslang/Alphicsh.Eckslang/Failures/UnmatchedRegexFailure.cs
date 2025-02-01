using System.Text.RegularExpressions;
using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Failures;

public class UnmatchedRegexFailure : BaseEckslangFailure
{
    public Regex ExpectedExpression { get; }
    public string ActualString { get; }

    public UnmatchedRegexFailure(Regex expected, string actual, IEckslangCursor cursor) : base("UnmatchedRegex", cursor)
    {
        ExpectedExpression = expected;
        ActualString = actual;

        Message = $"Could not match regular expression '{expected}' around the string '{actual}'.";
    }

    private const int MaxActualLength = 30;

    public static EckslangFailureGenerator GeneratorFor(Regex expected)
    {
        return (IEckslangScanner scanner) =>
        {
            var actual = scanner.Head.Length <= MaxActualLength
                ? scanner.Head.TrimEnd('\0').ToString()
                : scanner.Head.Slice(0, MaxActualLength).ToString() + "...";
            return new UnmatchedRegexFailure(expected, actual, scanner.Cursor);
        };
    }
}
