namespace Arpg.Editor;

public static class Window
{
  static TilesetPanel tileSet;


  public static void Initialize()
  {
    InitWindow(1280, 720, "Editor");
    SetTargetFPS(60);
    tileSet = new TilesetPanel("C:\\Users\\andar\\apps\\hamaka_studio\\arpg\\Arpg.Game\\Assets\\Textures\\TinyTown.png");
  }

  public static void Run()
  {
    Initialize();
    Loop();
    CloseWindow();
  }

  private static void Loop()
  {

    while (!WindowShouldClose())
    {
      Update();
      Draw();
    }
  }

  private static void Update()
  {
    tileSet.Update();
  }

  private static void Draw()
  {
    ClearBackground(Color.Black);
    BeginDrawing();
    tileSet.Draw();
    EndDrawing();
  }
}