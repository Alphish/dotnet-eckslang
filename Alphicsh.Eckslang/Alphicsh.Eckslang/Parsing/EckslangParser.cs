using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Parsing;

public class EckslangParser<TRun> : IEckslangParser
    where TRun : IEckslangParseRun<TRun>
{
    private IEckslangScanner Scanner { get; }
    private TRun Run { get; }

    public EckslangParser(IEckslangScanner scanner, TRun run)
    {
        Scanner = scanner;
        Run = run;
    }

    public void ParseNext()
    {
        if (Run.IsFinished)
            return;

        Run.CurrentStep(Scanner, Run);
    }

    public void ParseAll()
    {
        while (!Run.IsFinished)
        {
            ParseNext();
        }
    }
}
