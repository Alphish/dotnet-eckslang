using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Parsing;

public abstract class BaseEckslangFormat<TRun> : IEckslangFormat<TRun>
    where TRun : IEckslangParseRun<TRun>
{
    public IEckslangParser Parse(string content, TRun run)
    {
        var scanner = new EckslangScanner(content);
        PrepareRun(run);
        return new EckslangParser<TRun>(scanner, run);
    }

    protected abstract void PrepareRun(TRun run);
}
