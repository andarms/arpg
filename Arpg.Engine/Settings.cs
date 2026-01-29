namespace Arpg.Engine;

public static class Settings
{
  private static readonly Dictionary<string, object> dynamicSettings = [];

  // Strongly-typed property categories
  public static WindowSettings Window { get; } = new WindowSettings();
  public static TileSettings Tiles { get; } = new TileSettings();
  public static ZoomSettings Zoom { get; } = new ZoomSettings();

  static Settings()
  {
    InitializeDefaults();
  }

  private static void InitializeDefaults()
  {
    // Asset paths
    Set("AssetsPath", "Assets");
    Set("TexturesPath", "Assets/Textures");
    Set("FontsPath", "Assets/Fonts");
    Set("TilemapsPath", "Assets/Tilemaps");
    Set("DefaultTilesetPath", "Textures/TinyTown.png");

    // UI Settings
    Set("UIScale", 1.0f);
    Set("UIPadding", 20);

    // Game-specific
    Set("WorldLimitX", 5000f);
    Set("WorldLimitY", 3000f);

    // Editor-specific
    Set("GridEnabled", true);
    Set("CollisionGridSize", 8);
  }

  public static T Get<T>(string key)
  {
    if (dynamicSettings.TryGetValue(key, out var value) && value != null)
    {
      return (T)value;
    }
    return default(T)!;
  }

  public static void Set<T>(string key, T value)
  {
    if (value != null)
    {
      dynamicSettings[key] = value;
    }
  }
}

public class WindowSettings
{
  public int Width { get; set; } = 1280;
  public int Height { get; set; } = 720;
  public int FPS { get; set; } = 60;
}

public class TileSettings
{
  public int Size { get; set; } = 16;
  public int ScaledSize => Size * 2; // TileSize * Scale
}

public class ZoomSettings
{
  public float Default { get; set; } = 3.0f;
  public float Game { get; set; } = 3.0f;
  public float Editor { get; set; } = 2.0f;
}