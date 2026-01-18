using Arpg.Editor;

namespace Arpg.Editor;

public static class GameEditorViewModel
{
  public static int SelectedLayer { get; set; } = 0;
  public static int SelectedTool { get; set; } = 0;
}

public class GameEditor
{
  TilesetPanel tileset;
  MapPanel mapPanel = new();
  MiniMapPanel miniMapPanel = new();

  LayersToolbar layersToolbar = new();
  ToolToolbar toolToolbar = new();
  public GameEditor()
  {
    tileset = new TilesetPanel("C:\\Users\\andar\\apps\\hamaka_studio\\arpg\\Arpg.Game\\Assets\\Textures\\TinyTown.png");
  }

  public void Update()
  {
    tileset.Update();
    layersToolbar.Update();
    toolToolbar.Update();
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
