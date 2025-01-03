using Alphicsh.Eckslang.Scanning;

namespace Alphicsh.Eckslang.Parsing;

public delegate bool EckslangParseStep(IEckslangScanner scanner, IEckslangVisitor visitor);
