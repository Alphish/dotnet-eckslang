using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Parsing;

public class BaseEckslangParseRun<TRun> : IEckslangParseRun<TRun>
    where TRun : IEckslangParseRun<TRun>
{
    public EckslangParseStep<TRun> CurrentStep { get; private set; }
    private Stack<EckslangParseStep<TRun>> PendingSteps { get; }
    public bool IsFinished { get; private set; }

    public BaseEckslangParseRun()
    {
        CurrentStep = NoInitialStep;

        PendingSteps = new Stack<EckslangParseStep<TRun>>();
        PendingSteps.Push(Complete);
    }

    public StepCompletion ProceedWith(EckslangParseStep<TRun> step)
    {
        CurrentStep = step;
        return default;
    }

    public StepCompletion RunLater(EckslangParseStep<TRun> step)
    {
        PendingSteps.Push(step);
        return default;
    }

    public StepCompletion EnterScope(EckslangParseStep<TRun> onEnter, EckslangParseStep<TRun> onLeave)
    {
        CurrentStep = onEnter;
        PendingSteps.Push(onLeave);
        return default;
    }

    public StepCompletion LeaveScope()
    {
        CurrentStep = PendingSteps.Pop();
        return default;
    }

    private StepCompletion NoInitialStep(IEckslangScanner scanner, TRun run)
    {
        throw new InvalidOperationException($"No initial step was given to the parse state.");
    }

    protected StepCompletion Complete(IEckslangScanner scanner, TRun run)
    {
        if (!scanner.EndOfContent)
            throw new FormatException($"Unexpected content after end of parsing.");

        IsFinished = true;
        return default;
    }
}
