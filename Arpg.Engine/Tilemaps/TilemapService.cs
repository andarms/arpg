namespace Arpg.Engine.Tilemaps;

public static class TilemapService
{
  public static void Initialize()
  {
    // No longer needed since Settings is static
  }

  public static Tilemap CreateNew(int width, int height, string? tilesetPath = null)
  {
    string actualTilesetPath = tilesetPath ?? Settings.Get<string>("DefaultTilesetPath") ?? "Textures/TinyTown.png";
    string assetsBasePath = Settings.Get<string>("AssetsPath") ?? GetDefaultAssetsPath();
    string fullTilesetPath = Path.Combine(assetsBasePath, actualTilesetPath);

    if (!File.Exists(fullTilesetPath))
    {
      throw new FileNotFoundException($"Tileset file not found: {fullTilesetPath}");
    }

    Tileset tileset = new(LoadTexture(fullTilesetPath), 16, 16);
    return new Tilemap(width, height, tileset, actualTilesetPath);
  }

  public static Tilemap LoadFromFile(string filePath, string? assetsPath = null, string? gameAssetsPath = null)
  {
    try
    {
      string actualAssetsPath = assetsPath ?? Settings.Get<string>("TilemapsPath") ?? GetDefaultTilemapsPath();
      string actualGameAssetsPath = gameAssetsPath ?? Settings.Get<string>("AssetsPath") ?? GetDefaultAssetsPath();

      return Tilemap.Load(filePath, actualAssetsPath, actualGameAssetsPath);
    }
    catch (ArgumentNullException ex)
    {
      Console.WriteLine($"Tilemap loading failed - Null argument: {ex.Message}");
      throw;
    }
    catch (FileNotFoundException ex)
    {
      Console.WriteLine($"Tilemap loading failed - File not found: {ex.Message}");
      throw;
    }
    catch (InvalidDataException ex)
    {
      Console.WriteLine($"Tilemap loading failed - Invalid data: {ex.Message}");
      throw;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Tilemap loading failed - Unexpected error: {ex.Message}");
      throw;
    }
  }

  public static void SaveToFile(Tilemap tilemapData, string filePath, string? assetsPath = null)
  {
    string actualAssetsPath = assetsPath ?? Settings.Get<string>("TilemapsPath") ?? GetDefaultTilemapsPath();

    // Ensure directory exists
    Directory.CreateDirectory(actualAssetsPath);

    tilemapData.Save(filePath, actualAssetsPath);
  }

  private static string GetDefaultTilemapsPath()
  {
    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    string projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", ".."));
    return Path.Combine(projectRoot, "Arpg.Game", "Assets", "Rooms");
  }

  private static string GetDefaultAssetsPath()
  {
    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    string projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", ".."));
    return Path.Combine(projectRoot, "Arpg.Game", "Assets");
  }
}