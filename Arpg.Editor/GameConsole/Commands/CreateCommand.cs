using Arpg.Engine.GameConsole;
using Arpg.Engine.Scenes;

namespace Arpg.Editor.GameConsole.Commands;

public class CreateCommand : BaseCommand
{
  public CreateCommand()
      : base("create", "Create new resources", "create <type>", "type")
  {
  }

  public override string[]? Execute(string[] args, ICommandContext context)
  {
    ScenesController.PushScene<NewRoomScene>();
    return ["Opened room creation dialog. Fill in the details and press Enter to create."];
  }
}