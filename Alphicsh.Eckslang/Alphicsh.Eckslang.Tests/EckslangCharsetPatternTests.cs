using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Tests;

public class EckslangCharsetPatternTests
{
    [Fact]
    public void ShouldReadCorrectSpanSequence()
    {
        GivenContent("The quick_brown_fox\tjumps over 42apples\r\nand 99 bottles of H2O");
        ShouldReadPattern("variable", "The");
        ShouldReadPattern("space", " ");
        ShouldReadPattern("variable", "quick_brown_fox");
        ShouldReadPattern("space", "\t");
        ShouldReadPattern("variable", "jumps");
        ShouldReadPattern("space", " ");
        ShouldReadPattern("variable", "over");
        ShouldReadPattern("space", " ");
        ShouldReadPattern("number", "42");
        ShouldReadPattern("variable", "apples");
        ShouldReadPattern("space", "\r\n");
        ShouldReadPattern("variable", "and");
        ShouldReadPattern("space", " ");
        ShouldReadPattern("number", "99");
        ShouldReadPattern("space", " ");
        ShouldReadPattern("variable", "bottles");
        ShouldReadPattern("space", " ");
        ShouldReadPattern("variable", "of");
        ShouldReadPattern("space", " ");
        ShouldReadPattern("variable", "H2O");
    }

    private static EckslangArrayCharset NumericCharset { get; } = EckslangArrayCharset.FromCharacterClass(@"[0-9]");
    private static EckslangArrayCharset AlphabeticCharset { get; } = EckslangArrayCharset.FromCharacterClass(@"[A-Za-z_]");
    private static EckslangArrayCharset AlphanumericCharset { get; } = EckslangArrayCharset.FromCharacterClass(@"[0-9A-Za-z_]");
    private static EckslangArrayCharset SpaceCharset { get; } = EckslangArrayCharset.FromCharacterClass(@"\s");

    private static IEckslangPattern VariablePattern { get; } = new EckslangCharsetPattern(AlphanumericCharset, AlphabeticCharset);
    private static IEckslangPattern NumberPattern { get; } = new EckslangCharsetPattern(NumericCharset);
    private static IEckslangPattern SpacePattern { get; } = new EckslangCharsetPattern(SpaceCharset);

    private IEckslangScanner Scanner { get; set; } = default!;

    private void GivenContent(string content)
    {
        Scanner = new EckslangScanner(content);
    }

    private (string, string) ReadPattern()
    {
        var variable = Scanner.ReadPattern(VariablePattern);
        if (variable.Length > 0)
            return ("variable", variable.ToString());

        var number = Scanner.ReadPattern(NumberPattern);
        if (number.Length > 0)
            return ("number", number.ToString());

        var space = Scanner.ReadPattern(SpacePattern);
        if (space.Length > 0)
            return ("space", space.ToString());

        throw new FormatException("Neither variable, number nor space format matched scanned string.");
    }

    private void ShouldReadPattern(string expectedType, string expectedContent)
    {
        var (actualType, actualContent) = ReadPattern();
        Assert.Equal(expectedType, actualType);
        Assert.Equal(expectedContent, actualContent);
    }
}
