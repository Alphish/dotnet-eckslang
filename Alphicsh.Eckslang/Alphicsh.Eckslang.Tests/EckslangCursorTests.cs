using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Tests;

public class EckslangCursorTests
{
    [Fact]
    public void ShouldReadSimpleString()
    {
        GivenTail("");
        GivenSpan("Hello, world!");
        GivenHead("");
        WhenScannerMoved();
        ThenInitialCursorShouldBe(position: 0, line: 1, column: 1);
        ThenAdvanceCursorShouldBe(position: 13, line: 1, column: 14);
        ThenBacktrackCursorShouldMatchInitialCursor();
    }

    [Fact]
    public void ShouldHandleLfNewlines()
    {
        GivenTail("Lorem\nipsum ");
        GivenSpan("dolor\nsit\namet,");
        GivenHead(" consectetur\nadipiscing\nelit");
        WhenScannerMoved();
        ThenInitialCursorShouldBe(position: 12, line: 2, column: 7);
        ThenAdvanceCursorShouldBe(position: 27, line: 4, column: 6);
        ThenBacktrackCursorShouldMatchInitialCursor();
    }

    [Fact]
    public void ShouldHandleCrNewlines()
    {
        GivenTail("Lorem\ripsum ");
        GivenSpan("dolor\rsit\ramet,");
        GivenHead(" consectetur\radipiscing\relit");
        WhenScannerMoved();
        ThenInitialCursorShouldBe(position: 12, line: 2, column: 7);
        ThenAdvanceCursorShouldBe(position: 27, line: 4, column: 6);
        ThenBacktrackCursorShouldMatchInitialCursor();
    }

    [Fact]
    public void ShouldHandleCrlfNewlines()
    {
        GivenTail("Lorem\r\nipsum ");
        GivenSpan("dolor\r\nsit\r\namet,");
        GivenHead(" consectetur\r\nadipiscing\r\nelit");
        WhenScannerMoved();
        ThenInitialCursorShouldBe(position: 13, line: 2, column: 7);
        ThenAdvanceCursorShouldBe(position: 30, line: 4, column: 6);
        ThenBacktrackCursorShouldMatchInitialCursor();
    }

    [Fact]
    public void ShouldDoubleLfcrNewlines()
    {
        GivenTail("Lorem\n\ripsum ");
        GivenSpan("dolor\n\rsit\n\ramet,");
        GivenHead(" consectetur\n\radipiscing\n\relit");
        WhenScannerMoved();
        ThenInitialCursorShouldBe(position: 13, line: 3, column: 7);
        ThenAdvanceCursorShouldBe(position: 30, line: 7, column: 6);
        ThenBacktrackCursorShouldMatchInitialCursor();
    }

    [Fact]
    public void ShouldPauseMidCrlfAdvance()
    {
        GivenTail("Lorem ipsum ");
        GivenSpan("dolor sit amet,\r");
        GivenHead("\n consectetur adipiscing elit");
        WhenScannerMoved();
        ThenInitialCursorShouldBe(position: 12, line: 1, column: 13);
        ThenAdvanceCursorShouldBe(position: 28, line: 1, column: 29);
        ThenBacktrackCursorShouldMatchInitialCursor();
    }

    [Fact]
    public void ShouldAddLinePastCrWithoutLfAdvance()
    {
        GivenTail("Lorem ipsum ");
        GivenSpan("dolor sit amet,\r");
        GivenHead(" consectetur adipiscing elit");
        WhenScannerMoved();
        ThenInitialCursorShouldBe(position: 12, line: 1, column: 13);
        ThenAdvanceCursorShouldBe(position: 28, line: 2, column: 1);
        ThenBacktrackCursorShouldMatchInitialCursor();
    }

    [Fact]
    public void ShouldAddLinePastLastCrAdvance()
    {
        GivenTail("Lorem ipsum ");
        GivenSpan("dolor sit amet,\r");
        GivenHead("");
        WhenScannerMoved();
        ThenInitialCursorShouldBe(position: 12, line: 1, column: 13);
        ThenAdvanceCursorShouldBe(position: 28, line: 2, column: 1);
        ThenBacktrackCursorShouldMatchInitialCursor();
    }

    [Fact]
    public void ShouldPauseMidCrlfBacktrack()
    {
        GivenTail("Lorem ipsum\r");
        GivenSpan("\ndolor sit amet,");
        GivenHead(" consectetur adipiscing elit");
        WhenScannerMoved();
        ThenInitialCursorShouldBe(position: 12, line: 1, column: 13);
        ThenAdvanceCursorShouldBe(position: 28, line: 2, column: 16);
        ThenBacktrackCursorShouldMatchInitialCursor();
    }

    [Fact]
    public void ShouldSubstractLinePastCrWithoutLfBacktrack()
    {
        GivenTail("Lorem ipsum ");
        GivenSpan("\ndolor sit amet,");
        GivenHead(" consectetur adipiscing elit");
        WhenScannerMoved();
        ThenInitialCursorShouldBe(position: 12, line: 1, column: 13);
        ThenAdvanceCursorShouldBe(position: 28, line: 2, column: 16);
        ThenBacktrackCursorShouldMatchInitialCursor();
    }

    [Fact]
    public void ShouldSubtractLinePastLastCrBacktrack()
    {
        GivenTail("");
        GivenSpan("\ndolor sit amet,");
        GivenHead(" consectetur adipiscing elit");
        WhenScannerMoved();
        ThenInitialCursorShouldBe(position: 0, line: 1, column: 1);
        ThenAdvanceCursorShouldBe(position: 16, line: 2, column: 16);
        ThenBacktrackCursorShouldMatchInitialCursor();
    }

    // -------
    // Helpers
    // -------

    private string Tail { get; set; } = default!;
    private string Span { get; set; } = default!;
    private string Head { get; set; } = default!;

    private IEckslangCursor InitialCursor { get; set; } = default!;
    private IEckslangCursor AdvanceCursor { get; set; } = default!;
    private IEckslangCursor BacktrackCursor { get; set; } = default!;

    private void GivenTail(string tail) => Tail = tail;
    private void GivenSpan(string span) => Span = span;
    private void GivenHead(string head) => Head = head;

    private void WhenScannerMoved()
    {
        var scanner = new EckslangScanner(Tail + Span + Head);

        scanner.JumpTo(Tail.Length);
        InitialCursor = scanner.Cursor;

        scanner.JumpTo(Tail.Length + Span.Length);
        AdvanceCursor = scanner.Cursor;

        scanner.JumpTo(Tail.Length);
        BacktrackCursor = scanner.Cursor;
    }

    private void ThenInitialCursorShouldBe(int position, int line, int column)
        => Assert.Equal(new EckslangCursor(position, line, column), InitialCursor);

    private void ThenAdvanceCursorShouldBe(int position, int line, int column)
        => Assert.Equal(new EckslangCursor(position, line, column), AdvanceCursor);

    private void ThenBacktrackCursorShouldMatchInitialCursor()
        => Assert.Equal(InitialCursor, BacktrackCursor);
}
