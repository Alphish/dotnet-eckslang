using Alphicsh.Eckslang.Parsing;
using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Tests.Parsing;

internal class TestVisitor : IEckslangVisitor
{
    private Queue<KeyValuePair<string, string>> EntriesList { get; } = new Queue<KeyValuePair<string, string>>();

    public void Visit(string type, ReadOnlySpan<char> span, object? value, IEckslangCursor? cursorFrom, IEckslangCursor? cursorTo)
    {
        EntriesList.Enqueue(KeyValuePair.Create(type, span.ToString()));
    }

    public void Expect(string type, string value = "")
    {
        var entry = EntriesList.Dequeue();
        Assert.Equal(type, entry.Key);
        Assert.Equal(value, entry.Value);
    }
}
