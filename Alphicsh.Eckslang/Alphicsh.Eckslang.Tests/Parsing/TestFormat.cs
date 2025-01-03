using Alphicsh.Eckslang.Parsing;
using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Tests.Parsing;

internal class TestFormat : BaseEckslangFormat<TestFormat>
{
    public IEckslangPattern WordPattern { get; } = new EckslangRegexPattern(@"\w+");
    public IEckslangPattern OpenParenthesisPattern { get; } = new EckslangStringPattern("(");
    public IEckslangPattern CloseParenthesisPattern { get; } = new EckslangStringPattern(")");
    public IEckslangPattern CommaPattern { get; } = new EckslangStringPattern(",");
    public IEckslangPattern SpacePattern { get; } = new EckslangRegexPattern(@"\s+");

    protected override EckslangParseStep GetRootStep(IEckslangParser<TestFormat> parser)
    {
        return new TestExpressionReader(parser).ReadWord;
    }
}
