using System.Text;

namespace Arpg.Engine.Console.Commands;

public class HelpCommand(CommandRegistry registry) : BaseCommand("help", "Show available commands or help for a specific command", "help [command]", "?", "h")
{
  private readonly CommandRegistry registry = registry;

  public override string[]? Execute(string[] args, ICommandContext context)
  {
    if (args.Length == 0)
    {
      // Show all commands
      var commands = registry.GetCommands().OrderBy(cmd => cmd.Name);
      var lines = new List<string>
      {
        "Available commands:",
        ""
      };

      foreach (var cmd in commands)
      {
        lines.Add($"  {cmd.Name,-15} - {cmd.Description}");
      }

      lines.Add("");
      lines.Add("Type 'help <command>' for detailed help on a specific command.");

      return lines.ToArray();
    }
    else
    {
      // Show help for specific command
      var commandName = args[0].ToLower();
      var command = registry.GetCommands().FirstOrDefault(cmd =>
          cmd.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase) ||
          cmd.Aliases.Any(alias => alias.Equals(commandName, StringComparison.OrdinalIgnoreCase)));

      if (command == null)
      {
        context.OutputError($"Unknown command: {commandName}");
        return null;
      }

      var lines = new List<string>
      {
        $"Command: {command.Name}",
        $"Description: {command.Description}",
        $"Usage: {command.Usage}"
      };

      if (command.Aliases.Length > 0)
      {
        lines.Add($"Aliases: {string.Join(", ", command.Aliases)}");
      }

      return lines.ToArray();
    }
  }
}

public class ClearCommand : BaseCommand
{
  public ClearCommand()
      : base("clear", "Clear the console output", "clear", "cls")
  {
  }

  public override string[]? Execute(string[] args, ICommandContext context)
  {
    // This will need to be handled by the UI layer
    context.ContextData["ClearConsole"] = true;
    return null;
  }
}


public class ExitCommand : BaseCommand
{
  public ExitCommand()
      : base("exit", "Exit the application or close console", "exit", "quit", "q")
  {
  }

  public override string[]? Execute(string[] args, ICommandContext context)
  {
    context.ContextData["ExitRequested"] = true;
    return ["Goodbye!"];
  }
}