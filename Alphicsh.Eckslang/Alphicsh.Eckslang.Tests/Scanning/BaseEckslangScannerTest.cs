using Alphicsh.Eckslang.Failures;
using Alphicsh.Eckslang.Scanning;
using Shouldly;

namespace Alphicsh.Eckslang.Tests.Scanning;

public abstract class BaseEckslangScannerTest
{
    protected IEckslangScanner Scanner { get; set; } = default!;

    protected bool? CheckResult { get; set; } = null;
    protected string ReadString { get; set; } = default!;
    protected EckslangFailureEventArgs? ReceivedFailureEvent { get; set; }

    // ------
    // Givens
    // ------

    protected void GivenContent(string content)
    {
        Scanner = new EckslangScanner(content);
        Scanner.ScanFailed += (sender, e) => ReceivedFailureEvent = e;
    }

    protected void GivenPosition(int position)
    {
        Scanner.JumpTo(position);
    }

    // -----
    // Whens
    // -----

    protected abstract void WhenCheckedAhead();
    protected abstract void WhenSkipAttempted();
    protected abstract void WhenSkipExpected();
    protected abstract void WhenPeeked();
    protected abstract void WhenReadAttempted();
    protected abstract void WhenReadExpected();

    // -----
    // Thens
    // -----

    protected void ThenCheckResultShouldBe(bool expected)
        => CheckResult.ShouldBe(expected);

    protected void ThenReadStringShouldBe(string expected)
        => ReadString.ShouldBe(expected);

    protected void ThenPositionShouldBe(int expected)
        => Scanner.Position.ShouldBe(expected);

    protected void ThenFailureShouldBeAbsent()
    {
        Scanner.IsFailed.ShouldBeFalse();
        Scanner.Failure.ShouldBeNull();
        ReceivedFailureEvent.ShouldBeNull();
    }

    protected void ThenFailureShouldBePresent()
    {
        Scanner.IsFailed.ShouldBeTrue();
        Scanner.Failure.ShouldNotBeNull();
        ReceivedFailureEvent.ShouldNotBeNull();
    }

    // ----------
    // Test bases
    // ----------

    protected void TestHasAhead(string content, int startPosition, string expectedRead)
    {
        GivenContent(content);
        GivenPosition(startPosition);
        WhenCheckedAhead();
        ThenCheckResultShouldBe(expectedRead.Length > 0);
        ThenPositionShouldBe(startPosition);
        ThenFailureShouldBeAbsent();
    }

    protected void TestTrySkip(string content, int startPosition, string expectedRead)
    {
        GivenContent(content);
        GivenPosition(startPosition);
        WhenSkipAttempted();
        ThenCheckResultShouldBe(expectedRead.Length > 0);
        ThenPositionShouldBe(startPosition + expectedRead.Length);
        ThenFailureShouldBeAbsent();
    }

    protected void TestExpect(string content, int startPosition, string expectedRead)
    {
        GivenContent(content);
        GivenPosition(startPosition);
        WhenSkipExpected();
        ThenPositionShouldBe(expectedRead.Length > 0 ? startPosition + expectedRead.Length : Scanner.Length);

        if (expectedRead.Length > 0)
            ThenFailureShouldBeAbsent();
        else
            ThenFailureShouldBePresent();
    }

    protected void TestPeek(string content, int startPosition, string expectedRead)
    {
        GivenContent(content);
        GivenPosition(startPosition);
        WhenPeeked();
        ThenReadStringShouldBe(expectedRead);
        ThenPositionShouldBe(startPosition);
        ThenFailureShouldBeAbsent();
    }

    protected void TestTryRead(string content, int startPosition, string expectedRead)
    {
        GivenContent(content);
        GivenPosition(startPosition);
        WhenReadAttempted();
        ThenReadStringShouldBe(expectedRead);
        ThenPositionShouldBe(startPosition + expectedRead.Length);
        ThenFailureShouldBeAbsent();
    }

    protected void TestRead(string content, int startPosition, string expectedRead)
    {
        GivenContent(content);
        GivenPosition(startPosition);
        WhenReadExpected();
        ThenReadStringShouldBe(expectedRead);
        ThenPositionShouldBe(expectedRead.Length > 0 ? startPosition + expectedRead.Length : Scanner.Length);

        if (expectedRead.Length > 0)
            ThenFailureShouldBeAbsent();
        else
            ThenFailureShouldBePresent();
    }
}
