namespace Alphicsh.Eckslang.Scanning;

public static class EckslangCommonCharsets
{
    public static IEckslangCharset AsciiWord { get; } = EckslangArrayCharset.FromCharacterClass("[_0-9A-Za-z]");
    public static IEckslangCharset AsciiLetterUnderscore { get; } = EckslangArrayCharset.FromCharacterClass("[_A-Za-z]");
    public static IEckslangCharset AsciiDigit { get; } = EckslangArrayCharset.FromCharacterClass("[0-9]");
    public static IEckslangCharset AsciiSpace { get; } = EckslangArrayCharset.FromCharacterClass(@"\s", 128).Fill();
}
