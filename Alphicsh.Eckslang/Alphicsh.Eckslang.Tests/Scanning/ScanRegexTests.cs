using System.Text.RegularExpressions;

namespace Alphicsh.Eckslang.Tests.Scanning;

public class ScanRegexTests : BaseEckslangScannerTest
{
    private static TheoryDataRow<string, int, Regex, string> CreateCase(string description, string content, int startPosition, string pattern, string expectedString)
        => new TheoryDataRow<string, int, Regex, string>(content, startPosition, new Regex(pattern), expectedString).WithTestDisplayName(description);

    public static IEnumerable<ITheoryDataRow> TestCases = new List<ITheoryDataRow>
    {
        CreateCase("ShouldHandleEmptyString", "", 0, @"\w+", ""),

        CreateCase("ShouldScanCorrectContentStart", "Hello, world!", 0, @"\w+", "Hello"),
        CreateCase("ShouldHaltInvalidContentStart", "(2 + 2) * 2", 0, @"\w+", ""),
        CreateCase("ShouldScanCorrectContentMiddle", "Hello, world!", 7, @"\w+", "world"),
        CreateCase("ShouldHaltInvalidContentMiddle", "(2 + 2) * 2", 3, @"\w+", ""),
        CreateCase("ShouldScanCorrectContentEnd", "(2 + 2) * 2", 10, @"\w+", "2"),
        CreateCase("ShouldHaltInvalidContentEnd", "Hello, world!", 12, @"\w+", ""),
        CreateCase("ShouldHaltPastContentEnd", "(2 + 2) * 2", 11, @"\w+", ""),

        CreateCase("ShouldScanWithMatchingSymbolAtStart", "aaron", 0, @"a+", "aa"),
        CreateCase("ShouldHaltWithNoMatchingSymbols", "bob", 0, @"a+", ""),
        CreateCase("ShouldHaltWithMatchingSymbolsLater", "charlie", 0, @"a+", ""),

        CreateCase("ShouldCorrectlyScanNegatedCharacterClassStart", "test.received.txt", 0, @"[^.]+", "test"),
        CreateCase("ShouldCorrectlyScanNegatedCharacterClassMiddle", "test.received.txt", 5, @"[^.]+", "received"),
        CreateCase("ShouldCorrectlyScanNegatedCharacterClassEnd", "test.received.txt", 14, @"[^.]+", "txt"), // it must not include the \0 at the end!
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldCheckHasAheadCorrectly(string content, int startPosition, Regex regex, string expectedString)
    {
        GivenInputRegex(regex);
        TestHasAhead(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldTrySkipCorrectly(string content, int startPosition, Regex regex, string expectedString)
    {
        GivenInputRegex(regex);
        TestTrySkip(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldExpectCorrectly(string content, int startPosition, Regex regex, string expectedString)
    {
        GivenInputRegex(regex);
        TestExpect(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldPeekCorrectly(string content, int startPosition, Regex regex, string expectedString)
    {
        GivenInputRegex(regex);
        TestPeek(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldTryReadCorrectly(string content, int startPosition, Regex regex, string expectedString)
    {
        GivenInputRegex(regex);
        TestTryRead(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldReadCorrectly(string content, int startPosition, Regex regex, string expectedString)
    {
        GivenInputRegex(regex);
        TestRead(content, startPosition, expectedString);
    }

    // -------
    // Helpers
    // -------

    private Regex InputRegex { get; set; } = default!;

    private void GivenInputRegex(Regex inputRegex)
        => InputRegex = inputRegex;

    protected override void WhenCheckedAhead()
        => CheckResult = Scanner.HasAhead(InputRegex);

    protected override void WhenSkipAttempted()
        => CheckResult = Scanner.TrySkip(InputRegex);

    protected override void WhenSkipExpected()
        => Scanner.Expect(InputRegex);

    protected override void WhenPeeked()
        => ReadString = Scanner.Peek(InputRegex).ToString();

    protected override void WhenReadAttempted()
        => ReadString = Scanner.TryRead(InputRegex).ToString();

    protected override void WhenReadExpected()
        => ReadString = Scanner.Read(InputRegex).ToString();
}
