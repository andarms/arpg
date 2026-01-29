namespace Arpg.Engine.Tilemaps;

public static class TilemapService
{
  public static void Initialize()
  {
    // No longer needed since Settings is static
  }

  public static TilemapData CreateNew(int width, int height, string? tilesetPath = null)
  {
    string actualTilesetPath = tilesetPath ?? Settings.Get<string>("DefaultTilesetPath");
    string fullTilesetPath = Path.Combine(Settings.Get<string>("AssetsPath"), actualTilesetPath);
    Tileset tileset = new(LoadTexture(fullTilesetPath), 16, 16);
    return new TilemapData(width, height, tileset, actualTilesetPath);
  }

  public static TilemapData LoadFromFile(string filePath, string? assetsPath = null, string? gameAssetsPath = null)
  {
    string actualAssetsPath = assetsPath ?? Settings.Get<string>("TilemapsPath");
    string actualGameAssetsPath = gameAssetsPath ?? Settings.Get<string>("AssetsPath");

    return TilemapData.Load(filePath, actualAssetsPath, actualGameAssetsPath);
  }

  public static void SaveToFile(TilemapData tilemapData, string filePath, string? assetsPath = null)
  {
    string actualAssetsPath = assetsPath ?? Settings.Get<string>("TilemapsPath");

    // Ensure directory exists
    Directory.CreateDirectory(actualAssetsPath);

    tilemapData.Save(filePath, actualAssetsPath);
  }
}