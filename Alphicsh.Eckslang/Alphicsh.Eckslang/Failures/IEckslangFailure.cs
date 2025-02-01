using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Failures;

public interface IEckslangFailure
{
    public string Cause { get; }
    public string Message { get; }
    public IEckslangCursor? Cursor { get; }
}
