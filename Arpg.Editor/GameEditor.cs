namespace Arpg.Editor;

public class GameEditor
{
  readonly TilesetPanel tileset = new();
  readonly MapPanel mapPanel = new();


  readonly LayersToolbar layersToolbar = new();
  readonly ToolToolbar toolToolbar = new();

  public GameEditor()
  {
    GameEditorViewModel.CreateTilemap(25, 20, "Textures/tileset.png");
    GameEditorViewModel.SelectedLayer = 0;
    GameEditorViewModel.SelectedTool = 0;
  }

  public void Update()
  {
    // Only update tileset when not on collision layer
    if (GameEditorViewModel.SelectedLayer != 3)
    {
      tileset.Update();
    }

    layersToolbar.Update();
    toolToolbar.Update();
    mapPanel.Update();
  }

  public void Draw()
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
