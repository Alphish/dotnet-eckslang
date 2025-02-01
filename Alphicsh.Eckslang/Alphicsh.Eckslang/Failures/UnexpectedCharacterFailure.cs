using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Failures;

public class UnexpectedCharacterFailure : BaseEckslangFailure
{
    public char ExpectedCharacter { get; }
    public char ActualCharacter { get; }

    public UnexpectedCharacterFailure(char expected, char actual, IEckslangCursor cursor) : base("UnexpectedCharacter", cursor)
    {
        ExpectedCharacter = expected;
        ActualCharacter = actual;

        Message = $"Expected character '{GetCharacterString(ExpectedCharacter)}' but got '{GetCharacterString(ActualCharacter)}' instead.";
    }

    private static string GetCharacterString(char character)
    {
        return SpecialCharacterStrings.TryGetValue(character, out var specialString) ? specialString : character.ToString();
    }

    private static IReadOnlyDictionary<char, string> SpecialCharacterStrings { get; } = new Dictionary<char, string>
    {
        ['\0'] = "\\0",
        ['\n'] = "\\n",
        ['\r'] = "\\r",
        ['\t'] = "\\t",
    };

    public static EckslangFailureGenerator GeneratorFor(char expected)
    {
        return (IEckslangScanner scanner) => new UnexpectedCharacterFailure(expected, scanner.CurrentCharacter, scanner.Cursor);
    }
}
