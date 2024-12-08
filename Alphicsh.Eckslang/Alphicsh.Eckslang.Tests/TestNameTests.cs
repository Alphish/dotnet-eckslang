namespace Alphicsh.Eckslang.Tests;

public class TestNameTests
{
    [Fact]
    public void ShouldGetCorrectTestName()
    {
        Assert.Equal("TEST", EckslangScanner.TestName);
    }
}