using Arpg.Engine.Console.Commands;

namespace Arpg.Engine.Console;

public class ConsoleManager
{
  private readonly CommandRegistry registry;
  private readonly CommandContext context;
  private readonly List<string> outputLines = new();
  private int maxOutputLines = 100;

  public CommandRegistry CommandRegistry => registry;
  public ICommandContext Context => context;

  public event Action? OnClearRequested;

  public event Action? OnExitRequested;

  public event Action<int>? OnLinesAdded;

  public int MaxOutputLines
  {
    get => maxOutputLines;
    set => maxOutputLines = Math.Max(10, value);
  }

  public IReadOnlyList<string> OutputLines => outputLines.AsReadOnly();

  public ConsoleManager()
  {
    registry = new CommandRegistry();
    context = new CommandContext();

    RegisterBuiltInCommands();
    SetupEventHandlers();
  }

  public ConsoleManager(params object[] services) : this()
  {
    foreach (var service in services)
    {
      context.RegisterService(service);
    }
  }

  public void ExecuteCommand(string input)
  {
    if (string.IsNullOrWhiteSpace(input))
    {
      return;
    }

    // Add the command input to output
    AddOutputLine($"> {input}");
    int linesAdded = 1;

    // Clear any previous output from context
    context.ClearOutput();

    // Execute the command
    registry.ExecuteCommand(input, context);

    // Add any output from the command
    foreach (var output in context.GetOutputBuffer())
    {
      AddOutputLine(output);
      linesAdded++;
    }

    // Handle special context data
    HandleContextData();

    // Notify UI to auto-scroll based on added lines
    OnLinesAdded?.Invoke(linesAdded);
  }

  public void RegisterCommand(ICommand command)
  {
    registry.RegisterCommand(command);
  }


  public void UnregisterCommand(string name)
  {
    registry.UnregisterCommand(name);
  }

  public void RegisterService<T>(T service) where T : class
  {
    context.RegisterService(service);
  }

  public IEnumerable<string> GetCommandSuggestions(string prefix)
  {
    return registry.GetSuggestions(prefix);
  }

  public void ClearOutput()
  {
    outputLines.Clear();
  }

  public void AddOutputLine(string line)
  {
    outputLines.Add(line);

    // Remove old lines if we exceed the limit
    while (outputLines.Count > maxOutputLines)
    {
      outputLines.RemoveAt(0);
    }
  }

  private void RegisterBuiltInCommands()
  {
    registry.RegisterCommand(new HelpCommand(registry));
    registry.RegisterCommand(new ClearCommand());
    registry.RegisterCommand(new ExitCommand());
  }

  private void SetupEventHandlers()
  {
    // We'll handle context data events here
  }

  private void HandleContextData()
  {
    if (context.ContextData.ContainsKey("ClearConsole"))
    {
      context.ContextData.Remove("ClearConsole");
      ClearOutput();
      OnClearRequested?.Invoke();
    }

    if (context.ContextData.ContainsKey("ExitRequested"))
    {
      context.ContextData.Remove("ExitRequested");
      OnExitRequested?.Invoke();
    }
  }
}