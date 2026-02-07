using Arpg.Editor.GameConsole;
using Arpg.Engine.Scenes;

namespace Arpg.Editor.RoomsEditor;

public class RoomEditorScene : Scene
{
  readonly TilesetPanel tileset = new();
  readonly MapPanel mapPanel = new();
  readonly LayersToolbar layersToolbar = new();
  readonly ToolToolbar toolToolbar = new();

  public RoomEditorScene()
  {
    BackgroundColor = Color.Black;
  }

  public override void Initialize()
  {
    base.Initialize();
  }

  public override void Update(float dt)
  {
    // Only update tileset when not on collision layer
    if (GameEditorViewModel.SelectedLayer != 3)
    {
      tileset.Update();
    }

    layersToolbar.Update();
    toolToolbar.Update();
    mapPanel.Update();


    if (IsKeyPressed(Constants.ConsoleToggleKey))
    {
      ScenesController.PushScene<ConsoleScene>();
    }
  }

  public override void Draw()
  {
    mapPanel.Draw();

    // Only draw tileset when not on collision layer
    if (GameEditorViewModel.SelectedLayer != 3)
    {
      tileset.Draw();
    }

    layersToolbar.Draw();
    toolToolbar.Draw();
  }
}
