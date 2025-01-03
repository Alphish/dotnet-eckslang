using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Parsing;

public class EckslangParser<TFormat> : IEckslangParser<TFormat>
    where TFormat : IEckslangFormat<TFormat>
{
    public TFormat Format { get; }
    private IEckslangScanner Scanner { get; }
    private IEckslangVisitor Visitor { get; }

    public EckslangParser(TFormat format, IEckslangScanner scanner, IEckslangVisitor visitor)
    {
        Format = format;
        Scanner = scanner;
        Visitor = visitor;
    }

    private Stack<EckslangParseStep> StepsStack { get; } = new Stack<EckslangParseStep>();
    public bool IsFinished { get; private set; } = false;

    public void ParseNext()
    {
        if (IsFinished)
            return;

        IsFinished = !StepsStack.TryPop(out var step) || step.Invoke(Scanner, Visitor);
    }

    public void ParseAll()
    {
        while (!IsFinished)
        {
            ParseNext();
        }
    }

    public void ScheduleStep(EckslangParseStep step)
    {
        StepsStack.Push(step);
    }

    public void ScheduleSteps(params EckslangParseStep[] steps)
    {
        foreach (var step in steps.Reverse())
            StepsStack.Push(step);
    }
}
