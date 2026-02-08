using Arpg.Editor.RoomsEditor;
using Arpg.Engine.GameConsole;
using Arpg.Engine.Scenes;

namespace Arpg.Editor.GameConsole.Commands;

public class ResizeCommand : BaseCommand
{
  public ResizeCommand() : base("resize", "Resize a resource", "resize") { }

  public override string[]? Execute(string[] args, ICommandContext context)
  {
    if (!ValidateArgCount(args, 2, context))
    {
      return ["Usage: resize <width> <height>"];
    }
    if (!int.TryParse(args[0], out int width) || !int.TryParse(args[1], out int height))
    {
      return ["Width and height must be integers."];
    }

    GameEditorViewModel.Tilemap?.Resize(width, height);
    return ["Room resized to " + width + "x" + height];
  }
}