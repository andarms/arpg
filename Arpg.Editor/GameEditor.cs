using Arpg.Editor;

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
