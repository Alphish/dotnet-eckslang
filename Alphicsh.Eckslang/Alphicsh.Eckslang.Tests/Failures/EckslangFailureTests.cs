using System.Text.RegularExpressions;
using Alphicsh.Eckslang.Failures;
using Alphicsh.Eckslang.Scanning;
using Shouldly;

namespace Alphicsh.Eckslang.Tests.Failures;

public class EckslangFailureTests
{
    [Fact]
    public void ShouldMakeCorrectFailureOnExpectingWrongCharacter()
    {
        GivenContent("lorem");
        GivenPosition(2);
        When(() => Scanner.Expect('s'));

        var failure = ThenFailureShouldBe<UnexpectedCharacterFailure>();
        failure.Cause.ShouldBe("UnexpectedCharacter");
        failure.Cursor.ShouldBe(new EckslangCursor(2, 1, 3));
        failure.ExpectedCharacter.ShouldBe('s');
        failure.ActualCharacter.ShouldBe('r');
        failure.Message.ShouldBe("Expected character 's' but got 'r' instead.");
    }

    [Fact]
    public void ShouldMakeCorrectFailureOnExpectingWrongString()
    {
        GivenContent("black\nwhite\ngray\nred");
        GivenPosition(12);
        When(() => Scanner.Expect("grey"));

        var failure = ThenFailureShouldBe<UnexpectedStringFailure>();
        failure.Cause.ShouldBe("UnexpectedString");
        failure.Cursor.ShouldBe(new EckslangCursor(12, 3, 1));
        failure.ExpectedString.ShouldBe("grey");
        failure.ActualString.ShouldBe("gray");
        failure.Message.ShouldBe("Expected string 'grey' but got 'gray' instead.");
    }

    [Fact]
    public void ShouldMakeCorrectFailureOnExpectingTooLongString()
    {
        GivenContent("big blue");
        GivenPosition(4);
        When(() => Scanner.Expect("bluebird"));

        var failure = ThenFailureShouldBe<UnexpectedStringFailure>();
        failure.Cause.ShouldBe("UnexpectedString");
        failure.Cursor.ShouldBe(new EckslangCursor(4, 1, 5));
        failure.ExpectedString.ShouldBe("bluebird");
        failure.ActualString.ShouldBe("blue");
        failure.Message.ShouldBe("Expected string 'bluebird' but got 'blue' instead.");
    }

    [Fact]
    public void ShouldMakeCorrectFailureOnExpectingWrongRegex()
    {
        GivenContent("This is some content");
        GivenPosition(8);

        var regex = new Regex(@"[0-9]+");
        When(() => Scanner.Expect(regex));

        var failure = ThenFailureShouldBe<UnmatchedRegexFailure>();
        failure.Cause.ShouldBe("UnmatchedRegex");
        failure.Cursor.ShouldBe(new EckslangCursor(8, 1, 9));
        failure.ExpectedExpression.ShouldBe(regex);
        failure.ActualString.ShouldBe("some content");
        failure.Message.ShouldBe("Could not match regular expression '[0-9]+' around the string 'some content'.");
    }

    [Fact]
    public void ShouldMakeShortenedFailureOnExpectingWrongRegexWithLongContent()
    {
        GivenContent("This is some really really really long content that just goes on and on and on...");
        GivenPosition(8);

        var regex = new Regex(@"[0-9]+");
        When(() => Scanner.Expect(regex));

        var failure = ThenFailureShouldBe<UnmatchedRegexFailure>();
        failure.Cause.ShouldBe("UnmatchedRegex");
        failure.Cursor.ShouldBe(new EckslangCursor(8, 1, 9));
        failure.ExpectedExpression.ShouldBe(regex);
        failure.ActualString.ShouldBe("some really really really long...");
        failure.Message.ShouldBe("Could not match regular expression '[0-9]+' around the string 'some really really really long...'.");
    }

    [Fact]
    public void ShouldThrowFailureExceptionWithGivenScannerSetup()
    {
        GivenContent("lorem");
        GivenPosition(2);
        Scanner.ThrowingOnFailure();

        Action attempt = () => Scanner.Expect('s');
        var exception = attempt.ShouldThrow<EckslangFailureException>();
        exception.Message.ShouldBe($"UnexpectedCharacter at Ln: 1, Col: 3, Pos: 2: Expected character 's' but got 'r' instead.");

        ReceivedFailure = exception.Failure;

        var failure = ThenFailureShouldBe<UnexpectedCharacterFailure>();
        failure.Cause.ShouldBe("UnexpectedCharacter");
        failure.Cursor.ShouldBe(new EckslangCursor(2, 1, 3));
        failure.ExpectedCharacter.ShouldBe('s');
        failure.ActualCharacter.ShouldBe('r');
        failure.Message.ShouldBe("Expected character 's' but got 'r' instead.");
    }

    // -------
    // Helpers
    // -------

    private IEckslangScanner Scanner { get; set; } = default!;
    private IEckslangFailure ReceivedFailure { get; set; } = default!;

    private void GivenContent(string content)
    {
        Scanner = new EckslangScanner(content);
        Scanner.ScanFailed += (sender, e) => ReceivedFailure = e.Failure;
    }

    private void GivenPosition(int position)
        => Scanner.JumpTo(position);

    private void When(Action action)
        => action();

    private TType ThenFailureShouldBe<TType>()
        => ReceivedFailure.ShouldNotBeNull().ShouldBeOfType<TType>();

    private void ThenCauseShouldBe(string cause)
        => ReceivedFailure.Cause.ShouldBe(cause);

    private void ThenCursorShouldBe(int line, int column, int position)
    {
        var cursor = ReceivedFailure.Cursor!;
        cursor.Line.ShouldBe(line);
        cursor.Column.ShouldBe(column);
        cursor.Position.ShouldBe(position);
    }

    private void ThenMessageShouldBe(string message)
        => ReceivedFailure.Message.ShouldBe(message);
}
