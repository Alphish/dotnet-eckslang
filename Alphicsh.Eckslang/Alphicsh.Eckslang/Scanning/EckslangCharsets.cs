using System.Text;
using System.Text.RegularExpressions;

namespace Alphicsh.Eckslang.Scanning;

public static class EckslangCharsets
{
    public static string FromCharacterClass(string characterClass, char maxCharacter = (char)127)
    {
        var characterClassRegex = new Regex(characterClass);
        return FromCharacterClass(characterClassRegex, maxCharacter);
    }

    public static string FromCharacterClass(Regex characterClass, char maxCharacter = (char)127)
    {
        var charsetBuilder = new StringBuilder();
        for (char character = (char)1; character <= maxCharacter; character++)
        {
            if (characterClass.IsMatch(character.ToString()))
                charsetBuilder.Append(character);
        }
        return charsetBuilder.ToString();
    }

    public static string AsciiWord { get; } = FromCharacterClass("[_0-9A-Za-z]");
    public static string AsciiLetterUnderscore { get; } = FromCharacterClass("[_A-Za-z]");
    public static string AsciiDigit { get; } = FromCharacterClass("[0-9]");
    public static string AsciiSpace { get; } = " \t\r\n";
}
