namespace Arpg.Engine.Tilemaps;

public static class TilemapService
{
  private const string DefaultAssetsPath = "C:/Users/andar/apps/hamaka_studio/arpg/Arpg.Game/Assets/Tilemaps";
  private const string DefaultTilesetPath = "C:\\Users\\andar\\apps\\hamaka_studio\\arpg\\Arpg.Game\\Assets\\Textures\\TinyTown.png";

  public static TilemapData CreateNew(int width, int height, string? tilesetPath = null)
  {
    string actualTilesetPath = tilesetPath ?? DefaultTilesetPath;
    Tileset tileset = new(LoadTexture(actualTilesetPath), 16, 16);
    return new TilemapData(width, height, tileset);
  }

  public static TilemapData LoadFromFile(string filePath, string? tilesetPath = null, string? assetsPath = null)
  {
    string actualTilesetPath = tilesetPath ?? DefaultTilesetPath;
    string actualAssetsPath = assetsPath ?? DefaultAssetsPath;

    Tileset tileset = new(LoadTexture(actualTilesetPath), 16, 16);
    return TilemapData.Load(filePath, actualAssetsPath, tileset);
  }

  public static void SaveToFile(TilemapData tilemapData, string filePath, string? assetsPath = null)
  {
    string actualAssetsPath = assetsPath ?? DefaultAssetsPath;

    // Ensure directory exists
    Directory.CreateDirectory(actualAssetsPath);

    tilemapData.Save(filePath, actualAssetsPath);
  }
}