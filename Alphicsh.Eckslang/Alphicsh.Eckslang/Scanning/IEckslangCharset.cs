namespace Alphicsh.Eckslang.Scanning;

public interface IEckslangCharset : IEnumerable<char>
{
    bool Contains(char c);
}
