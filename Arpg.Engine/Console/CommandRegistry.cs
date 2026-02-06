namespace Arpg.Engine.Console;

public class CommandRegistry
{
  private readonly Dictionary<string, ICommand> commands = new();
  private readonly List<string> commandHistory = new();

  public void RegisterCommand(ICommand command)
  {
    var name = command.Name.ToLower();
    commands[name] = command;

    // Register aliases
    foreach (var alias in command.Aliases)
    {
      commands[alias.ToLower()] = command;
    }
  }

  public void UnregisterCommand(string name)
  {
    var lowerName = name.ToLower();
    if (commands.TryGetValue(lowerName, out var command))
    {
      // Remove all names (including aliases) for this command
      var keysToRemove = commands.Where(kvp => kvp.Value == command)
                               .Select(kvp => kvp.Key)
                               .ToList();

      foreach (var key in keysToRemove)
      {
        commands.Remove(key);
      }
    }
  }

  public void ExecuteCommand(string input, ICommandContext context)
  {
    if (string.IsNullOrWhiteSpace(input))
    {
      return;
    }

    commandHistory.Add(input);

    var parts = ParseCommandString(input);
    if (parts.Length == 0)
    {
      return;
    }

    var commandName = parts[0].ToLower();
    var args = parts.Skip(1).ToArray();

    if (commands.TryGetValue(commandName, out var command))
    {
      try
      {
        var result = command.Execute(args, context);
        if (result != null)
        {
          foreach (var line in result)
          {
            context.Output(line);
          }
        }
      }
      catch (Exception ex)
      {
        context.OutputError($"Error executing command '{commandName}': {ex.Message}");
      }
    }
    else
    {
      context.OutputError($"Unknown command: {commandName}. Type 'help' for available commands.");
    }
  }

  public IEnumerable<ICommand> GetCommands()
  {
    return commands.Values
                  .GroupBy(cmd => cmd.Name)
                  .Select(group => group.First());
  }

  public IEnumerable<string> GetSuggestions(string prefix)
  {
    if (string.IsNullOrEmpty(prefix))
    {
      return commands.Keys;
    }

    var lowerPrefix = prefix.ToLower();
    return commands.Keys.Where(name => name.StartsWith(lowerPrefix));
  }

  public IReadOnlyList<string> GetHistory() => commandHistory.AsReadOnly();

  public void ClearHistory() => commandHistory.Clear();

  private string[] ParseCommandString(string input)
  {
    var parts = new List<string>();
    var currentPart = "";
    var inQuotes = false;
    var escapeNext = false;

    for (int i = 0; i < input.Length; i++)
    {
      var c = input[i];

      if (escapeNext)
      {
        currentPart += c;
        escapeNext = false;
      }
      else if (c == '\\')
      {
        escapeNext = true;
      }
      else if (c == '"')
      {
        inQuotes = !inQuotes;
      }
      else if (char.IsWhiteSpace(c) && !inQuotes)
      {
        if (!string.IsNullOrEmpty(currentPart))
        {
          parts.Add(currentPart);
          currentPart = "";
        }
      }
      else
      {
        currentPart += c;
      }
    }

    if (!string.IsNullOrEmpty(currentPart))
    {
      parts.Add(currentPart);
    }

    return parts.ToArray();
  }
}