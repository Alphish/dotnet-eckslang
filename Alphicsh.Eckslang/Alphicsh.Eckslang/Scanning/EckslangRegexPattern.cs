using System.Text.RegularExpressions;

namespace Alphicsh.Eckslang.Scanning;

public class EckslangRegexPattern : IEckslangPattern
{
    private Regex Expression { get; }

    public EckslangRegexPattern(string pattern)
    {
        // anchor to the head position
        if (!pattern.StartsWith(@"\G"))
            pattern = @"\G" + pattern;

        Expression = new Regex(pattern, RegexOptions.Compiled);
    }

    public EckslangRegexPattern(Regex expression)
    {
        Expression = expression;
    }

    public ReadOnlySpan<char> Match(IEckslangScanner scanner)
    {
        var head = scanner.Head;
        
        var matches = Expression.EnumerateMatches(head);
        if (!matches.MoveNext())
            return ReadOnlySpan<char>.Empty;

        var match = matches.Current;
        var result = head.Slice(match.Index, match.Length);
        if (match.Index != 0)
            throw new FormatException($"The Eckslang Regex pattern caught a match '{result}' at {scanner.Position + match.Index} when starting from {scanner.Position}.");

        return result;
    }
}
