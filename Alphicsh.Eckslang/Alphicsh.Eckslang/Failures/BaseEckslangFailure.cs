using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Failures;

public abstract class BaseEckslangFailure : IEckslangFailure
{
    public string Cause { get; }
    public IEckslangCursor? Cursor { get; }
    public string Message { get; protected set; }

    protected BaseEckslangFailure(string cause, IEckslangCursor? cursor)
    {
        Cause = cause;
        Cursor = cursor;
        Message = $"{cause} failure has occured.";
    }
}
