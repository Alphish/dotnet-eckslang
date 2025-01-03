using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Parsing;

public interface IEckslangVisitor
{
    void Visit(string type, ReadOnlySpan<char> span, object? value, IEckslangCursor? cursorFrom, IEckslangCursor? cursorTo);
}
