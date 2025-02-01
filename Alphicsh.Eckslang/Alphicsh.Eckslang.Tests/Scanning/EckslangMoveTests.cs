using Alphicsh.Eckslang.Scanning;
using Shouldly;

namespace Alphicsh.Eckslang.Tests.Scanning;

public class EckslangMoveTests
{
    [Fact]
    public void SkipNextShouldAdvanceByOneCharacter()
    {
        var scanner = new EckslangScanner("lorem ipsum");
        scanner.Position.ShouldBe(0);

        scanner.SkipNext();
        scanner.Position.ShouldBe(1);

        scanner.SkipNext();
        scanner.Position.ShouldBe(2);
    }

    [Fact]
    public void SkipNextShouldAdvanceUpToLength()
    {
        var scanner = new EckslangScanner("e");
        scanner.Position.ShouldBe(0);

        scanner.SkipNext();
        scanner.Position.ShouldBe(1);

        scanner.SkipNext();
        scanner.Position.ShouldBe(1);
    }

    [Fact]
    public void MoveByLengthShouldAdvanceByLength()
    {
        var scanner = new EckslangScanner("lorem ipsum");
        scanner.Position.ShouldBe(0);

        scanner.MoveBy(3);
        scanner.Position.ShouldBe(3);

        scanner.MoveBy(3);
        scanner.Position.ShouldBe(6);
    }

    [Fact]
    public void MoveByLengthShouldAdvanceUpToLength()
    {
        var scanner = new EckslangScanner("lorem ipsum");
        scanner.Position.ShouldBe(0);

        scanner.MoveBy(8);
        scanner.Position.ShouldBe(8);

        scanner.MoveBy(8);
        scanner.Position.ShouldBe("lorem ipsum".Length);
    }

    [Fact]
    public void MoveByLengthShouldBacktrackByNegativeLength()
    {
        var scanner = new EckslangScanner("lorem ipsum");
        scanner.Position.ShouldBe(0);

        scanner.MoveBy(10);
        scanner.Position.ShouldBe(10);

        scanner.MoveBy(-3);
        scanner.Position.ShouldBe(7);
    }

    [Fact]
    public void MoveByLengthShouldBacktrackDownToZero()
    {
        var scanner = new EckslangScanner("lorem ipsum");
        scanner.Position.ShouldBe(0);

        scanner.MoveBy(5);
        scanner.Position.ShouldBe(5);

        scanner.MoveBy(-10);
        scanner.Position.ShouldBe(0);
    }

    [Fact]
    public void MoveToPositionShouldMoveToPosition()
    {
        var scanner = new EckslangScanner("lorem ipsum");
        scanner.Position.ShouldBe(0);

        scanner.MoveTo(5);
        scanner.Position.ShouldBe(5);

        scanner.MoveTo(5);
        scanner.Position.ShouldBe(5);
    }

    [Fact]
    public void MoveToPositionShouldMoveUpToLength()
    {
        var scanner = new EckslangScanner("lorem ipsum");
        scanner.Position.ShouldBe(0);

        scanner.MoveTo(99);
        scanner.Position.ShouldBe("lorem ipsum".Length);
    }

    [Fact]
    public void MoveToCursorShouldMoveToCursorPosition()
    {
        var scanner = new EckslangScanner("lorem ipsum");
        scanner.Position.ShouldBe(0);

        var cursor = new EckslangCursor(5, 123, 456);
        scanner.MoveTo(cursor);
        scanner.Position.ShouldBe(5);

        scanner.MoveTo(cursor);
        scanner.Position.ShouldBe(5);
    }

    [Fact]
    public void MoveToCursorShouldSetCursorCoordinates()
    {
        var scanner = new EckslangScanner("lorem ipsum");
        scanner.Position.ShouldBe(0);

        var cursor = new EckslangCursor(5, 123, 456);
        scanner.MoveTo(cursor);

        scanner.Cursor.Position.ShouldBe(5);
        scanner.Cursor.Line.ShouldBe(123);
        scanner.Cursor.Column.ShouldBe(456);
    }

    [Fact]
    public void MoveToCursorShouldMoveUpToLength()
    {
        var scanner = new EckslangScanner("lorem ipsum");
        scanner.Position.ShouldBe(0);

        var cursor = new EckslangCursor(99, 123, 456);
        scanner.MoveTo(cursor);

        scanner.Position.ShouldBe("lorem ipsum".Length);
    }
}
