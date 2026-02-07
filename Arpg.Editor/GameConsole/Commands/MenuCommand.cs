using Arpg.Engine.GameConsole;
using Arpg.Engine.Scenes;

namespace Arpg.Editor.GameConsole.Commands;

public class MenuCommand : BaseCommand
{
  public MenuCommand()
      : base("menu", "Open the editor menu", "menu")
  {
  }

  public override string[]? Execute(string[] args, ICommandContext context)
  {
    ScenesController.SwitchTo<EditorMenuScene>();
    return ["Opened editor menu. Use arrow keys to navigate and Enter to select."];
  }
}