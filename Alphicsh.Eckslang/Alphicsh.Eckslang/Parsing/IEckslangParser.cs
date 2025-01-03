namespace Alphicsh.Eckslang.Parsing;

public interface IEckslangParser
{
    void ParseNext();
    void ParseAll();
    void ScheduleStep(EckslangParseStep step);
    void ScheduleSteps(params EckslangParseStep[] steps);
}

public interface IEckslangParser<TFormat> : IEckslangParser
    where TFormat : IEckslangFormat<TFormat>
{
    TFormat Format { get; }
}
