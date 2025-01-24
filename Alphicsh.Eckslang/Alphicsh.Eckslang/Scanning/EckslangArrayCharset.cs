using System.Collections;
using System.Text.RegularExpressions;

namespace Alphicsh.Eckslang.Scanning;

public class EckslangArrayCharset : IEckslangCharset
{
    private string Charset { get; }
    public bool[] Checks { get; }

    private EckslangArrayCharset(IEnumerable<bool> checks)
    {
        Checks = checks.ToArray();

        var characters = checks.Select((contains, character) => KeyValuePair.Create((char)character, contains))
            .Where(kvp => kvp.Value)
            .Select(kvp => kvp.Key)
            .ToArray(); ;
        Charset = new string(characters);
    }

    private EckslangArrayCharset(IEnumerable<bool> checks, string charset)
    {
        Checks = checks.ToArray();
        Charset = charset;
    }

    // --------
    // Creation
    // --------

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

    // -------
    // Methods
    // -------

    public bool Contains(char c) => Checks[c];
    public IEnumerator<char> GetEnumerator() => Charset.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Charset.GetEnumerator();

    public EckslangArrayCharset Expand(int length)
    {
        if (length < Checks.Length)
            throw new ArgumentOutOfRangeException("length");

        var expansion = Enumerable.Repeat(false, length - Checks.Length);
        return new EckslangArrayCharset(Checks.Concat(expansion), Charset);
    }

    public EckslangArrayCharset Fill() => Expand(char.MaxValue + 1);
}
