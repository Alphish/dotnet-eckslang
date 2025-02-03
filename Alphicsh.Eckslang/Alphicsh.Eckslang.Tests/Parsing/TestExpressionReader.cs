using Alphicsh.Eckslang.Parsing;
using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Tests.Parsing;

public class TestExpressionReader : EckslangReader<TestFormat>
{
    public TestExpressionReader(TestFormat format) : base(format) { }

    public StepCompletion ReadWord(IEckslangScanner scanner, TestParseRun run)
    {
        scanner.TrySkip(Format.SpacePattern);

        var word = scanner.Read(Format.WordPattern);
        run.VisitWord(word);

        scanner.TrySkip(Format.SpacePattern);
        return run.ProceedWith(TryOpenParenthesis);
    }

    public StepCompletion TryOpenParenthesis(IEckslangScanner scanner, TestParseRun run)
    {
        if (!scanner.TrySkip('('))
            return run.LeaveScope();

        run.OpenParam();

        scanner.TrySkip(Format.SpacePattern);
        if (scanner.CurrentCharacter == ')')
            return run.ProceedWith(CloseParenthesis);

        return run.EnterScope(ReadWord, ContinueParenthesis);
    }

    public StepCompletion ContinueParenthesis(IEckslangScanner scanner, TestParseRun run)
    {
        if (!scanner.TrySkip(','))
            return run.ProceedWith(CloseParenthesis);

        run.SeparateItem();

        scanner.TrySkip(Format.SpacePattern);
        if (scanner.CurrentCharacter == ')')
            return run.ProceedWith(CloseParenthesis);

        return run.EnterScope(ReadWord, ContinueParenthesis);
    }

    public StepCompletion CloseParenthesis(IEckslangScanner scanner, TestParseRun run)
    {
        scanner.Expect(')');
        run.CloseParam();

        scanner.TrySkip(Format.SpacePattern);
        return run.LeaveScope();
    }
}
