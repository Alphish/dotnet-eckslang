namespace Alphicsh.Eckslang.Failures;

public class EckslangFailureEventArgs
{
    public IEckslangFailure Failure { get; }

    public EckslangFailureEventArgs(IEckslangFailure failure)
    {
        Failure = failure;
    }
}
