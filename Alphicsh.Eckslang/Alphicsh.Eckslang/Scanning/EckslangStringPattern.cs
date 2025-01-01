namespace Alphicsh.Eckslang.Scanning;

public class EckslangStringPattern : IEckslangPattern
{
    private string String { get; }
    private StringComparison Comparison { get; }

    public EckslangStringPattern(string str, StringComparison comparison)
    {
        String = str;
        Comparison = comparison;
    }

    public EckslangStringPattern(string str) : this(str, StringComparison.Ordinal)
    {
    }

    public ReadOnlySpan<char> Match(IEckslangScanner scanner)
    {
        return scanner.Head.StartsWith(String, Comparison)
            ? scanner.Head.Slice(0, String.Length)
            : ReadOnlySpan<char>.Empty;
    }
}
