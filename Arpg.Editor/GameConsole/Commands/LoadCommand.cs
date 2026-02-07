using Arpg.Engine.GameConsole;
using Arpg.Engine.Scenes;

namespace Arpg.Editor.GameConsole.Commands;

public class LoadCommand : BaseCommand
{
  public LoadCommand() : base("load", "Load a resource", "load") { }

  public override string[]? Execute(string[] args, ICommandContext context)
  {
    ScenesController.PushScene<RoomsSelectionScene>();
    return ["Opened rooms selection scene. Use arrow keys to navigate and Enter to select a room."];
  }
}