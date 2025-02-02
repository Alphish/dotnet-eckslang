using System.Text.RegularExpressions;
using Alphicsh.Eckslang.Scanning;
using Shouldly;

namespace Alphicsh.Eckslang.Tests.Scanning;

public class EckslangCharsetsTests
{
    // --------
    // Creation
    // --------

    [Fact]
    public void ShouldCreateCharsetFromCharacterClassPattern()
    {
        var charset = EckslangCharsets.FromCharacterClass("[lorem ipsu]");
        var actual = charset.ToCharArray();

        var expected = "loremipsu ".ToCharArray();
        actual.ShouldBe(expected, ignoreOrder: true);
    }

    [Fact]
    public void ShouldCreateCharsetFromCharacterClassRegex()
    {
        var characterClass = new Regex("[lorem ipsu]", RegexOptions.IgnoreCase);
        var charset = EckslangCharsets.FromCharacterClass(characterClass);
        var actual = charset.ToCharArray();

        var expected = "loremipsuLOREMIPSU ".ToCharArray();
        actual.ShouldBe(expected, ignoreOrder: true);
    }

    // -------
    // Presets
    // -------

    public static IEnumerable<ITheoryDataRow> PresetCases { get; } = new[]
    {
        new TheoryDataRow<string, string>(EckslangCharsets.AsciiWord, "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"),
        new TheoryDataRow<string, string>(EckslangCharsets.AsciiDigit, "0123456789"),
        new TheoryDataRow<string, string>(EckslangCharsets.AsciiLetterUnderscore, "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"),
        new TheoryDataRow<string, string>(EckslangCharsets.AsciiSpace, " \t\r\n"),
    };

    [Theory]
    [MemberData(nameof(PresetCases))]
    public void ShouldHaveCorrectPreset(string actualString, string expectedString)
    {
        var actual = actualString.ToCharArray();
        var expected = expectedString.ToCharArray();
        actual.ShouldBe(expected, ignoreOrder: true);
    }
}
