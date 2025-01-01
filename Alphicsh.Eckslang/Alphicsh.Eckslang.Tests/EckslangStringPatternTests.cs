using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Tests;

public class EckslangStringPatternTests
{
    [Fact]
    public void ShouldReadLowercaseContent()
    {
        GivenContent("if(){}elseif(){}else{}");
        ShouldReadPattern(IfCaseSensitivePattern, "if");
        ShouldReadPattern(OpenParenthesisPattern, "(");
        ShouldReadPattern(CloseParenthesisPattern, ")");
        ShouldReadPattern(OpenBracePattern, "{");
        ShouldReadPattern(CloseBracePattern, "}");

        ShouldReadPattern(ElseCaseSensitivePattern, "else");
        ShouldReadPattern(IfCaseSensitivePattern, "if");
        ShouldReadPattern(OpenParenthesisPattern, "(");
        ShouldReadPattern(CloseParenthesisPattern, ")");
        ShouldReadPattern(OpenBracePattern, "{");
        ShouldReadPattern(CloseBracePattern, "}");

        ShouldReadPattern(ElseCaseSensitivePattern, "else");
        ShouldReadPattern(OpenBracePattern, "{");
        ShouldReadPattern(CloseBracePattern, "}");
        ShouldBeAtEnd();
    }

    [Fact]
    public void ShouldReaMixedCaseContent()
    {
        GivenContent("If(){}elSEif(){}else{}");
        ShouldReadPattern(IfPattern, "If");
        ShouldReadPattern(OpenParenthesisPattern, "(");
        ShouldReadPattern(CloseParenthesisPattern, ")");
        ShouldReadPattern(OpenBracePattern, "{");
        ShouldReadPattern(CloseBracePattern, "}");

        ShouldReadPattern(ElsePattern, "elSE");
        ShouldReadPattern(IfCaseSensitivePattern, "if");
        ShouldReadPattern(OpenParenthesisPattern, "(");
        ShouldReadPattern(CloseParenthesisPattern, ")");
        ShouldReadPattern(OpenBracePattern, "{");
        ShouldReadPattern(CloseBracePattern, "}");

        ShouldReadPattern(ElseCaseSensitivePattern, "else");
        ShouldReadPattern(OpenBracePattern, "{");
        ShouldReadPattern(CloseBracePattern, "}");
        ShouldBeAtEnd();
    }

    private static IEckslangPattern IfCaseSensitivePattern { get; } = new EckslangStringPattern("if");
    private static IEckslangPattern IfPattern { get; } = new EckslangStringPattern("if", StringComparison.OrdinalIgnoreCase);
    private static IEckslangPattern ElseCaseSensitivePattern { get; } = new EckslangStringPattern("else");
    private static IEckslangPattern ElsePattern { get; } = new EckslangStringPattern("else", StringComparison.OrdinalIgnoreCase);
    private static IEckslangPattern OpenParenthesisPattern { get; } = new EckslangStringPattern("(");
    private static IEckslangPattern CloseParenthesisPattern { get; } = new EckslangStringPattern(")");
    private static IEckslangPattern OpenBracePattern { get; } = new EckslangStringPattern("{");
    private static IEckslangPattern CloseBracePattern { get; } = new EckslangStringPattern("}");

    private static IEckslangPattern[] Patterns = new[]
    {
        IfCaseSensitivePattern, IfPattern,
        ElseCaseSensitivePattern, ElsePattern,
        OpenParenthesisPattern, CloseParenthesisPattern,
        OpenBracePattern, CloseBracePattern,
    };

    private IEckslangScanner Scanner { get; set; } = default!;

    private void GivenContent(string content)
    {
        Scanner = new EckslangScanner(content);
    }

    private (IEckslangPattern, string) ReadPattern()
    {
        foreach (var pattern in Patterns)
        {
            var result = Scanner.ReadSpan(pattern);
            if (!result.IsEmpty)
                return (pattern, result.ToString());
        }
        throw new FormatException("None of available patterns matched the scanned string.");
    }

    private void ShouldReadPattern(IEckslangPattern expectedPattern, string expectedContent)
    {
        var (actualPattern, actualContent) = ReadPattern();
        Assert.Equal(expectedPattern, actualPattern);
        Assert.Equal(expectedContent, actualContent);
    }

    private void ShouldBeAtEnd()
    {
        Assert.True(Scanner.Head.IsEmpty);
    }
}
