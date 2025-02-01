namespace Alphicsh.Eckslang.Tests.Scanning;

public class ScanStringDefaultComparisonTests : BaseEckslangScannerTest
{
    private static TheoryDataRow<string, int, string, string> CreateCase(string description, string content, int startPosition, string inputString, string expectedString)
        => new TheoryDataRow<string, int, string, string>(content, startPosition, inputString, expectedString).WithTestDisplayName(description);

    public static IEnumerable<ITheoryDataRow> TestCases = new List<ITheoryDataRow>
    {
        CreateCase("ShouldHandleEmptyString", "", 0, "add", ""),

        CreateCase("ShouldScanCorrectContentStart", "add element", 0, "add", "add"),
        CreateCase("ShouldHaltInvalidContentStart", "remove element", 0, "add", ""),
        CreateCase("ShouldScanCorrectContentMiddle", "common adder", 7, "add", "add"),
        CreateCase("ShouldHaltInvalidContentMiddle", "common viper", 7, "add", ""),
        CreateCase("ShouldScanCorrectContentEnd", "do add", 3, "add", "add"),
        CreateCase("ShouldHaltInvalidContentEnd", "do remove", 6, "add", ""),
        CreateCase("ShouldHaltPartiallyPastContentEnd", "nomad", 3, "add", ""),
        CreateCase("ShouldHaltFullyPastContentEnd", "nomad", 5, "add", ""),

        CreateCase("ShouldHaltInvalidContentCase", "COMMON ADDER", 7, "add", ""),
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldCheckHasAheadCorrectly(string content, int startPosition, string inputString, string expectedString)
    {
        GivenInputString(inputString);
        TestHasAhead(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldTrySkipCorrectly(string content, int startPosition, string inputString, string expectedString)
    {
        GivenInputString(inputString);
        TestTrySkip(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldExpectCorrectly(string content, int startPosition, string inputString, string expectedString)
    {
        GivenInputString(inputString);
        TestExpect(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldPeekCorrectly(string content, int startPosition, string inputString, string expectedString)
    {
        GivenInputString(inputString);
        TestPeek(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldTryReadCorrectly(string content, int startPosition, string inputString, string expectedString)
    {
        GivenInputString(inputString);
        TestTryRead(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldReadCorrectly(string content, int startPosition, string inputString, string expectedString)
    {
        GivenInputString(inputString);
        TestRead(content, startPosition, expectedString);
    }

    // -------
    // Helpers
    // -------

    private string InputString { get; set; } = default!;

    private void GivenInputString(string inputString)
        => InputString = inputString;

    protected override void WhenCheckedAhead()
        => CheckResult = Scanner.HasAhead(InputString);

    protected override void WhenSkipAttempted()
        => CheckResult = Scanner.TrySkip(InputString);

    protected override void WhenSkipExpected()
        => Scanner.Expect(InputString);

    protected override void WhenPeeked()
        => ReadString = Scanner.Peek(InputString).ToString();

    protected override void WhenReadAttempted()
        => ReadString = Scanner.TryRead(InputString).ToString();

    protected override void WhenReadExpected()
        => ReadString = Scanner.Read(InputString).ToString();
}
