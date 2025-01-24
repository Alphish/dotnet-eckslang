using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Parsing;

public delegate StepCompletion EckslangParseStep<TRun>(IEckslangScanner scanner, TRun run)
    where TRun : IEckslangParseRun<TRun>;
