namespace Arpg.Editor;

public static class Constants
{
  public static string TilesetPath = "C:\\Users\\andar\\apps\\hamaka_studio\\arpg\\Arpg.Game\\Assets\\Textures\\TinyTown.png";
  public const int TileSize = 16;
  public const int ScaledTileSize = TileSize * 2;
  public const float Scale = 2.0f;
  public const int Padding = 16;
  public const int Padding2 = 32;
  public const int Padding3 = 48;
  public const int Padding4 = 64;
  public static Font DefaultFont = LoadFont("Assets/Fonts/monogram-extended.ttf");
  public static Texture2D CursorTexture = LoadTexture("Assets/Textures/cursor.png");


  public static KeyboardKey ConsoleToggleKey = KeyboardKey.Escape;
}