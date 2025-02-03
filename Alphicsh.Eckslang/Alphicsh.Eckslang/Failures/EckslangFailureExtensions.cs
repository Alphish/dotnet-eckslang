using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Failures;

public static class EckslangFailureExtensions
{
    public static EckslangFailureException ToException(this IEckslangFailure failure)
    {
        return new EckslangFailureException(failure);
    }

    public static IEckslangScanner ThrowingOnFailure(this IEckslangScanner scanner)
    {
        scanner.ScanFailed += (sender, e) => throw e.Failure.ToException();
        return scanner;
    }
}
