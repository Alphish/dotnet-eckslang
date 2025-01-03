using Alphicsh.Eckslang.Parsing;
using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Tests.Parsing;

internal class TestExpressionReader : EckslangReader<TestFormat>
{
    public TestExpressionReader(IEckslangParser<TestFormat> parser) : base(parser) { }

    public bool ReadWord(IEckslangScanner scanner, IEckslangVisitor visitor)
    {
        scanner.ReadSpan(Format.SpacePattern);
        var word = scanner.ReadSpan(Format.WordPattern);
        visitor.Visit("call", word, null, null, null);
        scanner.ReadSpan(Format.SpacePattern);
        return ProceedWith(TryOpenParenthesis);
    }

    public bool TryOpenParenthesis(IEckslangScanner scanner, IEckslangVisitor visitor)
    {
        var openParenthesis = scanner.ReadSpan(Format.OpenParenthesisPattern);
        if (openParenthesis.IsEmpty)
            return Leave();

        visitor.Visit("begin_params", ReadOnlySpan<char>.Empty, null, null, null);
        var closeParenthesis = scanner.ReadSpan(Format.CloseParenthesisPattern);
        if (!closeParenthesis.IsEmpty)
        {
            visitor.Visit("end_params", ReadOnlySpan<char>.Empty, null, null, null);
            return Leave();
        }

        return Enter(ReadWord, ContinueParenthesis);
    }

    public bool ContinueParenthesis(IEckslangScanner scanner, IEckslangVisitor visitor)
    {
        var comma = scanner.ReadSpan(Format.CommaPattern);
        if (!comma.IsEmpty)
            return Enter(ReadWord, ContinueParenthesis);

        var parenthesis = scanner.ReadSpan(Format.CloseParenthesisPattern);
        visitor.Visit("end_params", ReadOnlySpan<char>.Empty, null, null, null);
        return Leave();
    }
}
