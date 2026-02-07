using Arpg.Engine;
using Arpg.Engine.GameConsole;
using Arpg.Engine.Scenes;

namespace Arpg.Editor.GameConsole.Commands;

public class LoadCommand : BaseCommand
{
  public LoadCommand()
      : base("load", "Load a resource", "load <info>")
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
        ScenesController.PushScene<RoomsSelectionScene>();
        return ["Opened rooms selection scene. Use arrow keys to navigate and Enter to select a room."];


      default:
        context.OutputError($"Unknown info type: {infoType}. Available: rooms");
        return null;
    }
  }
}