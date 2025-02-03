namespace Alphicsh.Eckslang.Tests.Scanning;

public class ScanStringCustomComparisonTests : BaseEckslangScannerTest
{
    private static TheoryDataRow<string, int, string, StringComparison, string> CreateCase(string description, string content, int startPosition, string inputString, StringComparison comparison, string expectedString)
        => new TheoryDataRow<string, int, string, StringComparison, string>(content, startPosition, inputString, comparison, expectedString).WithTestDisplayName(description);

    public static IEnumerable<ITheoryDataRow> TestCases = new List<ITheoryDataRow>
    {
        CreateCase("ShouldHandleEmptyString", "", 0, "add", StringComparison.OrdinalIgnoreCase, ""),

        CreateCase("ShouldScanCorrectContentStart", "add element", 0, "add", StringComparison.OrdinalIgnoreCase, "add"),
        CreateCase("ShouldHaltInvalidContentStart", "remove element", 0, "add", StringComparison.OrdinalIgnoreCase, ""),
        CreateCase("ShouldScanCorrectContentMiddle", "common adder", 7, "add", StringComparison.OrdinalIgnoreCase, "add"),
        CreateCase("ShouldHaltInvalidContentMiddle", "common viper", 7, "add", StringComparison.OrdinalIgnoreCase, ""),
        CreateCase("ShouldScanCorrectContentEnd", "do add", 3, "add", StringComparison.OrdinalIgnoreCase, "add"),
        CreateCase("ShouldHaltInvalidContentEnd", "do remove", 6, "add", StringComparison.OrdinalIgnoreCase, ""),
        CreateCase("ShouldHaltPartiallyPastContentEnd", "nomad", 3, "add", StringComparison.OrdinalIgnoreCase, ""),
        CreateCase("ShouldHaltFullyPastContentEnd", "nomad", 5, "add", StringComparison.OrdinalIgnoreCase, ""),

        CreateCase("ShouldScanCorrectContentWithIgnoreCase", "COMMON ADDER", 7, "add", StringComparison.OrdinalIgnoreCase, "ADD"),
        CreateCase("ShouldHaltInvalidContentCaseSensitive", "COMMON ADDER", 7, "add", StringComparison.Ordinal, ""),
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldCheckHasAheadCorrectly(string content, int startPosition, string inputString, StringComparison comparison, string expectedString)
    {
        GivenInputString(inputString);
        GivenComparison(comparison);
        TestHasAhead(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldTrySkipCorrectly(string content, int startPosition, string inputString, StringComparison comparison, string expectedString)
    {
        GivenInputString(inputString);
        GivenComparison(comparison);
        TestTrySkip(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldExpectCorrectly(string content, int startPosition, string inputString, StringComparison comparison, string expectedString)
    {
        GivenInputString(inputString);
        GivenComparison(comparison);
        TestExpect(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldPeekCorrectly(string content, int startPosition, string inputString, StringComparison comparison, string expectedString)
    {
        GivenInputString(inputString);
        GivenComparison(comparison);
        TestPeek(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldTryReadCorrectly(string content, int startPosition, string inputString, StringComparison comparison, string expectedString)
    {
        GivenInputString(inputString);
        GivenComparison(comparison);
        TestTryRead(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldReadCorrectly(string content, int startPosition, string inputString, StringComparison comparison, string expectedString)
    {
        GivenInputString(inputString);
        GivenComparison(comparison);
        TestRead(content, startPosition, expectedString);
    }

    // -------
    // Helpers
    // -------

    private string InputString { get; set; } = default!;
    private StringComparison Comparison { get; set; }

    private void GivenInputString(string inputString)
        => InputString = inputString;

    private void GivenComparison(StringComparison comparison)
        => Comparison = comparison;

    protected override void WhenCheckedAhead()
        => CheckResult = Scanner.HasAhead(InputString, Comparison);

    protected override void WhenSkipAttempted()
        => CheckResult = Scanner.TrySkip(InputString, Comparison);

    protected override void WhenSkipExpected()
        => Scanner.Expect(InputString, Comparison);

    protected override void WhenPeeked()
        => ReadString = Scanner.Peek(InputString, Comparison).ToString();

    protected override void WhenReadAttempted()
        => ReadString = Scanner.TryRead(InputString, Comparison).ToString();

    protected override void WhenReadExpected()
        => ReadString = Scanner.Read(InputString, Comparison).ToString();
}
