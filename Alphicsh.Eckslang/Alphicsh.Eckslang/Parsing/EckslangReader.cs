namespace Alphicsh.Eckslang.Parsing;

public class EckslangReader<TFormat> where TFormat : IEckslangFormat<TFormat>
{
    protected IEckslangParser<TFormat> Parser { get; }
    protected TFormat Format => Parser.Format;

    public EckslangReader(IEckslangParser<TFormat> parser)
    {
        Parser = parser;
    }

    protected bool ProceedWith(EckslangParseStep step)
    {
        Parser.ScheduleStep(step);
        return false;
    }

    protected bool Enter(EckslangParseStep nextStep, EckslangParseStep leaveStep)
    {
        Parser.ScheduleSteps(nextStep, leaveStep);
        return false;
    }

    protected bool Leave()
    {
        return false;
    }
}
