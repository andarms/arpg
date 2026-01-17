namespace Arpg.Game.Assets;

public static class AssetsManager
{
  const string ASSETS_PATH = "Assets/";
  public static readonly Dictionary<string, Texture2D> Textures = [];
  public static Font DefaultFont { get; private set; }


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
      Console.WriteLine($"Textures directory not found: {texturesPath}");
    }

    // Load fonts
    DefaultFont = LoadFont(Path.Combine(ASSETS_PATH, "Fonts/monogram-extended.ttf"));
  }

  internal static void UnloadAssets()
  {
    // Unload all loaded textures
    foreach (var texture in Textures.Values)
    {
      UnloadTexture(texture);
    }
    Textures.Clear();
  }
}