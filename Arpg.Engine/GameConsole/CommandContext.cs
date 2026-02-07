namespace Arpg.Engine.GameConsole;

public class CommandContext : ICommandContext
{
  private readonly Dictionary<Type, object> services = new();
  private readonly List<string> outputBuffer = new();

  public Dictionary<string, object> ContextData { get; } = new();


  public CommandContext()
  {
  }

  public CommandContext(params object[] initialServices)
  {
    foreach (var service in initialServices)
    {
      RegisterService(service);
    }
  }

  public void Output(string message)
  {
    outputBuffer.Add(message);
    System.Console.WriteLine(message); // Also output to regular console
  }

  public void OutputError(string message)
  {
    var errorMessage = $"ERROR: {message}";
    outputBuffer.Add(errorMessage);
    System.Console.WriteLine(errorMessage); // Also output to regular console
  }

  public T? GetService<T>() where T : class
  {
    return services.TryGetValue(typeof(T), out var service) ? service as T : null;
  }

  public bool HasService<T>() where T : class
  {
    return services.ContainsKey(typeof(T));
  }

  public void RegisterService<T>(T service) where T : class
  {
    services[typeof(T)] = service;
  }

  public void UnregisterService<T>() where T : class
  {
    services.Remove(typeof(T));
  }

  public IReadOnlyList<string> GetOutputBuffer() => outputBuffer.AsReadOnly();

  public void ClearOutput() => outputBuffer.Clear();
}