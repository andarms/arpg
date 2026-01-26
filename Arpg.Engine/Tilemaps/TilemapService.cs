namespace Arpg.Engine.Tilemaps;

public static class TilemapService
{
  private const string DefaultAssetsPath = "C:/Users/andar/apps/hamaka_studio/arpg/Arpg.Game/Assets/Tilemaps";
  private const string DefaultGameAssetsPath = "C:/Users/andar/apps/hamaka_studio/arpg/Arpg.Game/Assets";
  private const string DefaultTilesetPath = "Textures/TinyTown.png";

  public static TilemapData CreateNew(int width, int height, string? tilesetPath = null)
  {
    string actualTilesetPath = tilesetPath ?? DefaultTilesetPath;
    string fullTilesetPath = Path.Combine(DefaultGameAssetsPath, actualTilesetPath);
    Tileset tileset = new(LoadTexture(fullTilesetPath), 16, 16);
    return new TilemapData(width, height, tileset, actualTilesetPath);
  }

  public static TilemapData LoadFromFile(string filePath, string? assetsPath = null, string? gameAssetsPath = null)
  {
    string actualAssetsPath = assetsPath ?? DefaultAssetsPath;
    string actualGameAssetsPath = gameAssetsPath ?? DefaultGameAssetsPath;

    return TilemapData.Load(filePath, actualAssetsPath, actualGameAssetsPath);
  }

  public static void SaveToFile(TilemapData tilemapData, string filePath, string? assetsPath = null)
  {
    string actualAssetsPath = assetsPath ?? DefaultAssetsPath;

    // Ensure directory exists
    Directory.CreateDirectory(actualAssetsPath);

    tilemapData.Save(filePath, actualAssetsPath);
  }
}