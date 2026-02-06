using Arpg.Engine.Console;

namespace Arpg.Editor.GameConsole.Commands;


public class ShowCommand : BaseCommand
{
  public ShowCommand()
      : base("show", "Show information about commands or editor state", "show <info>", "info")
  {
  }

  public override string[]? Execute(string[] args, ICommandContext context)
  {
    if (!ValidateArgCount(args, 1, context))
    {
      return null;
    }

    var infoType = args[0].ToLower();

    switch (infoType)
    {
      case "rooms":
        return new[]
        {
          "Rooms:",
          "- Room1",
          "- Room2",
          "- Room3"
        }; // Example output

      default:
        context.OutputError($"Unknown info type: {infoType}. Available: rooms");
        return null;
    }
  }
}