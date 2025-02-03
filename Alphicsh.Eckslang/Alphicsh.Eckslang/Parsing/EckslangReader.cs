namespace Alphicsh.Eckslang.Parsing;

public class EckslangReader<TFormat> where TFormat : IEckslangFormat
{
    protected TFormat Format { get; }

    public EckslangReader(TFormat format)
    {
        Format = format;
    }
}
