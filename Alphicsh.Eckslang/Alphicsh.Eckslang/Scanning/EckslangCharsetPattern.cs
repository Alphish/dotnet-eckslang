namespace Alphicsh.Eckslang.Scanning;

public class EckslangCharsetPattern : IEckslangPattern
{
    private IEckslangCharset MainCharset { get; }
    private IEckslangCharset InitialCharset { get; }

    public EckslangCharsetPattern(IEckslangCharset mainCharset, IEckslangCharset? initialCharset = null)
    {
        MainCharset = mainCharset;
        InitialCharset = initialCharset ?? mainCharset;
    }

    public ReadOnlySpan<char> Match(IEckslangScanner scanner)
    {
        var head = scanner.Head;
        if (!InitialCharset.Contains(head[0]))
            return ReadOnlySpan<char>.Empty;

        var len = 1;
        while (len < head.Length && MainCharset.Contains(head[len]))
            len++;

        return head.Slice(0, len);
    }
}
