using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Tests;

public class EckslangRegexPatternTests
{
    [Fact]
    public void ShouldReadCorrectSpanSequence()
    {
        GivenContent("lorem ipsum\r\n123 snake_case");
        ShouldReadPattern(WordPattern, "lorem");
        ShouldReadPattern(SpacePattern, " ");
        ShouldReadPattern(WordPattern, "ipsum");
        ShouldReadPattern(SpacePattern, "\r\n");
        ShouldReadPattern(WordPattern, "123");
        ShouldReadPattern(SpacePattern, " ");
        ShouldReadPattern(WordPattern, "snake_case");
    }

    // -------
    // Helpers
    // -------

    private static IEckslangPattern WordPattern { get; } = new EckslangRegexPattern(@"\w+");
    private static IEckslangPattern SpacePattern { get; } = new EckslangRegexPattern(@"\s+");

    private IEckslangScanner Scanner { get; set; } = default!;

    private void GivenContent(string content)
    {
        Scanner = new EckslangScanner(content);
    }

    private void ShouldReadPattern(IEckslangPattern pattern, string expected)
    {
        var actual = Scanner.ReadPattern(pattern).ToString();
        Assert.Equal(expected, actual);
    }
}