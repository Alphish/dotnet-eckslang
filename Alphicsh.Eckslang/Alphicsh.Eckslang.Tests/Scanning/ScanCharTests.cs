namespace Alphicsh.Eckslang.Tests.Scanning;

public class ScanCharTests : BaseEckslangScannerTest
{
    private static TheoryDataRow<string, int, char, string> CreateCase(string description, string content, int startPosition, char character, string expectedString)
        => new TheoryDataRow<string, int, char, string>(content, startPosition, character, expectedString).WithTestDisplayName(description);

    public static IEnumerable<ITheoryDataRow> TestCases = new List<ITheoryDataRow>
    {
        CreateCase("ShouldHandleEmptyString", "", 0, 'l', ""),

        CreateCase("ShouldScanCorrectContentStart", "lorem", 0, 'l', "l"),
        CreateCase("ShouldHaltInvalidContentStart", "lorem", 0, 'o', ""),
        CreateCase("ShouldScanCorrectContentMiddle", "lorem", 2, 'r', "r"),
        CreateCase("ShouldHaltInvalidContentMiddle", "lorem", 2, 'e', ""),
        CreateCase("ShouldScanCorrectContentEnd", "lorem", 4, 'm', "m"),
        CreateCase("ShouldHaltInvalidContentEnd", "lorem", 4, 'e', ""),
        CreateCase("ShouldHaltPastContentEnd", "lorem", 5, 'm', ""),
    };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldCheckHasAheadCorrectly(string content, int startPosition, char character, string expectedString)
    {
        GivenCharacter(character);
        TestHasAhead(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldTrySkipCorrectly(string content, int startPosition, char character, string expectedString)
    {
        GivenCharacter(character);
        TestTrySkip(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldExpectCorrectly(string content, int startPosition, char character, string expectedString)
    {
        GivenCharacter(character);
        TestExpect(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldPeekCorrectly(string content, int startPosition, char character, string expectedString)
    {
        GivenCharacter(character);
        TestPeek(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldTryReadCorrectly(string content, int startPosition, char character, string expectedString)
    {
        GivenCharacter(character);
        TestTryRead(content, startPosition, expectedString);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ShouldReadCorrectly(string content, int startPosition, char character, string expectedString)
    {
        GivenCharacter(character);
        TestRead(content, startPosition, expectedString);
    }

    // -------
    // Helpers
    // -------

    private char Character { get; set; }

    private void GivenCharacter(char character)
        => Character = character;

    protected override void WhenCheckedAhead()
        => CheckResult = Scanner.HasAhead(Character);

    protected override void WhenSkipAttempted()
        => CheckResult = Scanner.TrySkip(Character);

    protected override void WhenSkipExpected()
        => Scanner.Expect(Character);

    protected override void WhenPeeked()
        => ReadString = Scanner.Peek(Character).ToString();

    protected override void WhenReadAttempted()
        => ReadString = Scanner.TryRead(Character).ToString();

    protected override void WhenReadExpected()
        => ReadString = Scanner.Read(Character).ToString();
}
