using System.Text.RegularExpressions;

namespace Alphicsh.Eckslang.Scanning;

public class EckslangArrayCharset : IEckslangCharset
{
    public bool[] Values { get; }

    // --------
    // Creation
    // --------

    public EckslangArrayCharset(IEnumerable<bool> values)
    {
        Values = values.ToArray();
    }

    public static EckslangArrayCharset FromString(string characters, int length = char.MaxValue + 1)
    {
        var values = Enumerable.Repeat(false, length).ToArray();
        foreach (var character in characters.AsSpan())
        {
            values[character] = true;
        }
        return new EckslangArrayCharset(values);
    }

    public static EckslangArrayCharset FromCharacterClass(Regex classExpression, int length = char.MaxValue + 1)
    {
        var values = Enumerable.Range(char.MinValue, length)
            .Select(c => classExpression.IsMatch(((char)c).ToString()))
            .ToArray();
        return new EckslangArrayCharset(values);
    }

    public static EckslangArrayCharset FromCharacterClass(string classPattern, int length = char.MaxValue + 1)
        => FromCharacterClass(new Regex(classPattern), length);

    public static EckslangArrayCharset FromCharset(IEckslangCharset charset, int length = char.MaxValue + 1)
    {
        var values = Enumerable.Range(char.MinValue, length)
            .Select(c => charset.Contains((char)c))
            .ToArray();
        return new EckslangArrayCharset(values);
    }

    // ------
    // Checks
    // ------

    public bool Contains(char c)
    {
        return Values[c];
    }
}
