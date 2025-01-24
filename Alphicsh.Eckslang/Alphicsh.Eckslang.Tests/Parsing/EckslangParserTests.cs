using Alphicsh.Eckslang.Parsing;

namespace Alphicsh.Eckslang.Tests.Parsing;

public class EckslangParserTests
{
    [Fact]
    public void ShouldParseValidExpression()
    {
        GivenContent("lorem(ipsum(dolor(), sit), amet)");

        WhenParsed();

        ThenExpectVisit("word:lorem");
        ThenExpectVisit("param_open");
        ThenExpectVisit("word:ipsum");
        ThenExpectVisit("param_open");
        ThenExpectVisit("word:dolor");
        ThenExpectVisit("param_open");
        ThenExpectVisit("param_close");
        ThenExpectVisit("separate");
        ThenExpectVisit("word:sit");
        ThenExpectVisit("param_close");
        ThenExpectVisit("separate");
        ThenExpectVisit("word:amet");
        ThenExpectVisit("param_close");
        Assert.Empty(Run.VisitLog);
    }

    private string Content { get; set; } = default!;
    private TestParseRun Run { get; set; } = default!;
    private EckslangParser<TestParseRun> Parser { get; set; } = default!;

    private void GivenContent(string content)
    {
        Content = content;
    }

    private void WhenParsed()
    {
        var format = new TestFormat();
        Run = new TestParseRun();
        var parser = format.Parse(Content, Run);
        parser.ParseAll();
    }

    private void ThenExpectVisit(string expected)
    {
        var actual = Run.VisitLog.Dequeue();
        Assert.Equal(expected, actual);
    }
}
