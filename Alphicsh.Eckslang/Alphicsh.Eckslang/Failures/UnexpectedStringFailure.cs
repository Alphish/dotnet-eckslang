using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Failures;

public class UnexpectedStringFailure : BaseEckslangFailure
{
    public string ExpectedString { get; }
    public string ActualString { get; }

    public UnexpectedStringFailure(string expected, string actual, IEckslangCursor cursor) : base("UnexpectedString", cursor)
    {
        ExpectedString = expected;
        ActualString = actual;

        Message = $"Expected string '{expected}' but got '{actual}' instead.";
    }

    public static EckslangFailureGenerator GeneratorFor(string expected)
    {
        return (IEckslangScanner scanner) =>
        {
            var actual = scanner.Head.Slice(0, Math.Min(scanner.Head.Length, expected.Length)).TrimEnd('\0').ToString();
            return new UnexpectedStringFailure(expected, actual, scanner.Cursor);
        };
    }
}
