using Arpg.Editor.Utils;

namespace Arpg.Editor;

public static class GameEditorViewModel
{
  public static int SelectedLayer { get; set; }
  public static int SelectedTool { get; set; } = 0;
  public static bool ShowGrid { get; set; } = true;


  // Layer fading settings
  public static bool EnableLayerFading { get; set; } = true;
  public static float InactiveLayerOpacity { get; set; } = 0.3f;

  private static TilesetViewModel? tileset;
  public static TilesetViewModel Tileset => tileset ?? throw new Exception("Tileset not initialized");
  public static TilemapViewModel? Tilemap { get; set; }

  public static void CreateTilemap(int width, int height, string? tilesetPath = null)
  {
    string actualTilesetPath = tilesetPath ?? throw new Exception("Tileset path must be provided when creating a new tilemap");
    string fullPath = FilePathService.GetAssetPath(actualTilesetPath);

    Tilemap = new TilemapViewModel();
    tileset = new TilesetViewModel(fullPath);
    Tilemap.NewMap(width, height, actualTilesetPath);
  }

  public static void LoadTilemap(string filePath)
  {
    Tilemap = new TilemapViewModel();
    Tilemap.Load(filePath);

    // Update tileset based on loaded map's tileset
    if (Tilemap.Data?.TilesetPath is not null)
    {
      string fullTilesetPath = FilePathService.GetAssetPath(Tilemap.Data.TilesetPath);
      tileset = new TilesetViewModel(fullTilesetPath);
    }
  }
}
