using System.Text.RegularExpressions;
using Alphicsh.Eckslang.Parsing;

namespace Alphicsh.Eckslang.Tests.Parsing;

public class TestFormat : BaseEckslangFormat<TestParseRun>
{
    public TestExpressionReader Reader { get; }
    public Regex WordPattern { get; } = new Regex(@"\G\w+", RegexOptions.Compiled);
    public Regex SpacePattern { get; } = new Regex(@"\G\s+", RegexOptions.Compiled);

    public TestFormat()
    {
        Reader = new TestExpressionReader(this);
    }

    protected override void PrepareRun(TestParseRun run)
    {
        run.ProceedWith(Reader.ReadWord);
    }
}
