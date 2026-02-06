namespace Arpg.Engine.Console;

public interface ICommandContext
{

  void Output(string message);

  void OutputError(string message);

  T? GetService<T>() where T : class;

  bool HasService<T>() where T : class;

  Dictionary<string, object> ContextData { get; }
}