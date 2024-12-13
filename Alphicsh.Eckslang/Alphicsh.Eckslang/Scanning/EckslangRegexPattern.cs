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

        Expression = new Regex(pattern);
    }

    public EckslangRegexPattern(Regex expression)
    {
        Expression = expression;
    }

    public ReadOnlySpan<char> Match(IEckslangScanner scanner)
    {
        var head = scanner.Head;
        
        var matches = Expression.EnumerateMatches(head);
        matches.MoveNext();
        var match = matches.Current;

        return head.Slice(match.Index, match.Length);
    }
}
