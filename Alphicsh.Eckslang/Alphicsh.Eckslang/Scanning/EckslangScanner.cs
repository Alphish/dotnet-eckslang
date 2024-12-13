namespace Alphicsh.Eckslang.Scanning;

public class EckslangScanner : IEckslangScanner
{
    public string Content { get; }
    public int Position { get; set; }

    public EckslangScanner(string content)
    {
        Content = content;
        Position = 0;
    }

    public ReadOnlySpan<char> Head => Content.AsSpan(Position);

    public ReadOnlySpan<char> ReadSpan(IEckslangPattern pattern)
    {
        var match = pattern.Match(this);
        Position += match.Length;
        return match;
    }
}
