using Alphicsh.Eckslang.Parsing;
using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Tests.Parsing;

public class TestExpressionReader : EckslangReader<TestFormat>
{
    public TestExpressionReader(TestFormat format) : base(format) { }

    public StepCompletion ReadWord(IEckslangScanner scanner, TestParseRun run)
    {
        scanner.SkipRegex(Format.SpacePattern);

        var word = scanner.ReadRegex(Format.WordPattern);
        run.VisitWord(word);

        scanner.SkipRegex(Format.SpacePattern);
        return run.ProceedWith(TryOpenParenthesis);
    }

    public StepCompletion TryOpenParenthesis(IEckslangScanner scanner, TestParseRun run)
    {
        if (!scanner.TrySkipChar('('))
            return run.LeaveScope();

        run.OpenParam();

        scanner.SkipRegex(Format.SpacePattern);
        if (scanner.CurrentCharacter == ')')
            return run.ProceedWith(CloseParenthesis);

        return run.EnterScope(ReadWord, ContinueParenthesis);
    }

    public StepCompletion ContinueParenthesis(IEckslangScanner scanner, TestParseRun run)
    {
        if (!scanner.TrySkipChar(','))
            return run.ProceedWith(CloseParenthesis);

        run.SeparateItem();

        scanner.SkipRegex(Format.SpacePattern);
        if (scanner.CurrentCharacter == ')')
            return run.ProceedWith(CloseParenthesis);

        return run.EnterScope(ReadWord, ContinueParenthesis);
    }

    public StepCompletion CloseParenthesis(IEckslangScanner scanner, TestParseRun run)
    {
        scanner.ExpectChar(')');
        run.CloseParam();

        scanner.SkipRegex(Format.SpacePattern);
        return run.LeaveScope();
    }
}
