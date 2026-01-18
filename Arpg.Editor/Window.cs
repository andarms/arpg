namespace Arpg.Editor;

public static class Window
{
  static GameEditor editor;

  public static void Initialize()
  {
    InitWindow(1280, 720, "Editor");
    SetTargetFPS(60);
    editor = new GameEditor();

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
    editor.Update();
  }

  private static void Draw()
  {
    ClearBackground(Color.Black);
    BeginDrawing();
    editor.Draw();
    EndDrawing();
  }
}