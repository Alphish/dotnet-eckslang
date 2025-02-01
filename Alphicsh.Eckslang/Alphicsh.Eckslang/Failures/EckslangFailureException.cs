namespace Alphicsh.Eckslang.Failures;

public class EckslangFailureException : Exception
{
    public IEckslangFailure Failure { get; }

    public EckslangFailureException(IEckslangFailure failure) : base($"{failure.Cause} at {failure.Cursor}: {failure.Message}")
    {
        Failure = failure;
    }
}
