namespace Alphicsh.Eckslang.Parsing;

public interface IEckslangFormat
{
}

public interface IEckslangFormat<TFormat> : IEckslangFormat
    where TFormat : IEckslangFormat<TFormat>
{
    void SetupParser(IEckslangParser<TFormat> parser, IEckslangVisitor visitor);
}
