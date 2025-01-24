namespace Alphicsh.Eckslang.Parsing;

public interface IEckslangParseRun<TRun> where TRun : IEckslangParseRun<TRun>
{
    EckslangParseStep<TRun> CurrentStep { get; }
    bool IsFinished { get; }

    StepCompletion ProceedWith(EckslangParseStep<TRun> step);
    StepCompletion RunLater(EckslangParseStep<TRun> step);
    StepCompletion EnterScope(EckslangParseStep<TRun> onEnter, EckslangParseStep<TRun> onLeave);
    StepCompletion LeaveScope();
}
