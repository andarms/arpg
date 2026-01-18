using Arpg.Editor;

namespace Arpg.Editor;

public static class GameEditorViewModel
{
  public static int SelectedLayer { get; set; } = 0;
  public static int SelectedTool { get; set; } = 0;
  static TilesetViewModel? tileset;
  public static TilesetViewModel Tileset => tileset ?? throw new Exception("Tileset not initialized");
  public static TilemapViewModel? Tilemap { get; set; }

  public static void CreateTilemap(int width, int height)
  {
    Tilemap = new TilemapViewModel(width, height);
    tileset = new TilesetViewModel("C:\\Users\\andar\\apps\\hamaka_studio\\arpg\\Arpg.Game\\Assets\\Textures\\TinyTown.png");
  }
}

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
    GameEditorViewModel.Tilemap?.SetTile(GameEditorViewModel.SelectedLayer, 0, 0, 1);
    GameEditorViewModel.Tilemap?.SetTile(GameEditorViewModel.SelectedLayer, 1, 0, 2);
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
