using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Parsing;

public abstract class BaseEckslangFormat<TFormat> : IEckslangFormat<TFormat>
    where TFormat : IEckslangFormat<TFormat>
{
    public void SetupParser(IEckslangParser<TFormat> parser, IEckslangVisitor visitor)
    {
        parser.ScheduleSteps(GetRootStep(parser), FinishParsing);
    }

    protected abstract EckslangParseStep GetRootStep(IEckslangParser<TFormat> parser);

    protected virtual bool FinishParsing(IEckslangScanner scanner, IEckslangVisitor visitor)
    {
        if (!scanner.EndOfContent)
            throw new FormatException($"Unexpected content after end of parsing.");

        visitor.Visit("end", ReadOnlySpan<char>.Empty, null, null, null);
        return true;
    }
}
