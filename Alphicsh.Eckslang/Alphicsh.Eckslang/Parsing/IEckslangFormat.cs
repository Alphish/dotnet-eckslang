namespace Alphicsh.Eckslang.Parsing;

public interface IEckslangFormat
{
}

public interface IEckslangFormat<TFlow> : IEckslangFormat
{
    IEckslangParser Parse(string content, TFlow flow);
}
