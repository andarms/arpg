namespace Arpg.Engine.Assets;

public static class AssetsManager
{
  private static string ASSETS_PATH = "Assets/";
  public static readonly Dictionary<string, Texture2D> Textures = [];
  public static readonly Dictionary<string, List<int>> Maps = [];
  public static Font DefaultFont { get; private set; }

  public static void SetAssetBasePath(string basePath)
  {
    ASSETS_PATH = basePath.EndsWith('/') ? basePath : basePath + "/";
  }


  public static void LoadAssets()
  {
    // Load all textures from Assets/Textures recursively
    string texturesPath = Path.Combine(ASSETS_PATH, "Textures");
    if (Directory.Exists(texturesPath))
    {
      var textureFiles = Directory.GetFiles(texturesPath, "*.png", SearchOption.AllDirectories);
      foreach (var file in textureFiles)
      {
        string relativePath = Path.GetRelativePath(texturesPath, file);
        string key = relativePath.Replace('\\', '/');
        key = key[0..^4];

        Texture2D texture = LoadTexture(file);
        Textures[key] = texture;
      }
    }
    else
    {
      System.Console.WriteLine($"Textures directory not found: {texturesPath}");
    }

    // Load fonts
    DefaultFont = LoadFont(Path.Combine(ASSETS_PATH, "Fonts/monogram-extended.ttf"));


    string tilemapsPath = Path.Combine(ASSETS_PATH, "Tilemaps");
    if (Directory.Exists(tilemapsPath))
    {
      var tilemapFiles = Directory.GetFiles(tilemapsPath, "*.txt", SearchOption.AllDirectories);
      foreach (var file in tilemapFiles)
      {
        string relativePath = Path.GetRelativePath(tilemapsPath, file);
        string key = relativePath.Replace('\\', '/');
        key = key[0..^4];

        // For simplicity, assume all tilemaps are 20x15
        List<int> tiles = LoadTilemapLayer(file, 20, 15);
        Maps[key] = tiles;
      }
    }
    else
    {
      System.Console.WriteLine($"Tilemaps directory not found: {tilemapsPath}");
    }
  }


  public static List<int> LoadTilemapLayer(string filePath, int width, int height)
  {
    List<int> tiles = [];
    if (File.Exists(filePath))
    {
      var lines = File.ReadAllLines(filePath);
      foreach (var line in lines)
      {
        var tileIndices = line.Split(' ').Select(s => int.Parse(s.Trim()));
        tiles.AddRange(tileIndices);
      }
    }
    else
    {
      System.Console.WriteLine($"Tilemap layer file not found: {filePath}");
    }
    return tiles;
  }

  public static void UnloadAssets()
  {
    // Unload all loaded textures
    foreach (var texture in Textures.Values)
    {
      UnloadTexture(texture);
    }
    Textures.Clear();
  }
}