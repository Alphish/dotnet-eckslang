using Alphicsh.Eckslang.Parsing;
using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Tests.Parsing;

public class EckslangParserTests
{
    [Fact]
    public void ShouldParseValidExpression()
    {
        GivenContent("lorem(ipsum(dolor(), sit), amet)");
        WhenParsed();
        ThenExpectVisit("call", "lorem");
        ThenExpectVisit("begin_params");
        ThenExpectVisit("call", "ipsum");
        ThenExpectVisit("begin_params");
        ThenExpectVisit("call", "dolor");
        ThenExpectVisit("begin_params");
        ThenExpectVisit("end_params");
        ThenExpectVisit("call", "sit");
        ThenExpectVisit("end_params");
        ThenExpectVisit("call", "amet");
        ThenExpectVisit("end_params");
        ThenExpectVisit("end");
    }

    private EckslangParser<TestFormat> Parser { get; set; } = default!;
    private TestVisitor Visitor { get; set; } = default!;

    private void GivenContent(string content)
    {
        var scanner = new EckslangScanner(content);
        Visitor = new TestVisitor();
        Parser = new EckslangParser<TestFormat>(new TestFormat(), scanner, Visitor);
        Parser.Format.SetupParser(Parser, Visitor);
    }

    private void WhenParsed()
    {
        Parser.ParseAll();
    }

    private void ThenExpectVisit(string type, string value = "")
    {
        Visitor.Expect(type, value);
    }
}
