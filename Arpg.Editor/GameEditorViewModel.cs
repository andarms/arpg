namespace Arpg.Editor;

public static class GameEditorViewModel
{
  public static int SelectedLayer { get; set; }
  public static int SelectedTool { get; set; } = 0;
  public static bool ShowGrid { get; set; } = true;

  private static TilesetViewModel? tileset;
  public static TilesetViewModel Tileset => tileset ?? throw new Exception("Tileset not initialized");
  public static TilemapViewModel? Tilemap { get; set; }

  public static void CreateTilemap(int width, int height, string? tilesetPath = null)
  {
    string actualTilesetPath = tilesetPath ?? "C:\\Users\\andar\\apps\\hamaka_studio\\arpg\\Arpg.Game\\Assets\\Textures\\TinyTown.png";

    Tilemap = new TilemapViewModel();
    tileset = new TilesetViewModel(actualTilesetPath);
    Tilemap.NewMap(width, height);
  }
}
