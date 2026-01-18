public static class Settings
{
  public static string TilesetPath = "C:\\Users\\andar\\apps\\hamaka_studio\\arpg\\Arpg.Game\\Assets\\Textures\\TinyTown.png";
  public const int TileSize = 16;
  public const int ScaledTileSize = TileSize * 2;
  public const int Padding = 16;
  public static Font DefaultFont = LoadFont("Assets/Fonts/monogram-extended.ttf");
  public static Texture2D CursorTexture = LoadTexture("Assets/Textures/cursor.png");
}