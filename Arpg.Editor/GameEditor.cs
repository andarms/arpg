namespace Arpg.Editor;

public class GameEditor
{
  readonly TilesetPanel tileset = new();
  readonly MapPanel mapPanel = new();
  readonly MiniMapPanel miniMapPanel = new();

  readonly LayersToolbar layersToolbar = new();
  readonly ToolToolbar toolToolbar = new();

  public GameEditor()
  {
    GameEditorViewModel.CreateTilemap(25, 12);
    GameEditorViewModel.SelectedLayer = 0;
    GameEditorViewModel.SelectedTool = 0;
  }

  public void Update()
  {
    tileset.Update();
    layersToolbar.Update();
    toolToolbar.Update();
    mapPanel.Update();
  }

  public void Draw()
  {
    mapPanel.Draw();
    tileset.Draw();
    miniMapPanel.Draw();
    layersToolbar.Draw();
    toolToolbar.Draw();
  }
}
