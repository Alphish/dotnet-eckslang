using Alphicsh.Eckslang.Parsing;

namespace Alphicsh.Eckslang.Tests.Parsing;

public class TestParseRun : BaseEckslangParseRun<TestParseRun>
{
    public Queue<string> VisitLog { get; } = new Queue<string>();

    public void VisitWord(ReadOnlySpan<char> word)
    {
        VisitLog.Enqueue("word:" + word.ToString());
    }

    public void OpenParam()
    {
        VisitLog.Enqueue("param_open");
    }

    public void CloseParam()
    {
        VisitLog.Enqueue("param_close");
    }

    public void SeparateItem()
    {
        VisitLog.Enqueue("separate");
    }
}
