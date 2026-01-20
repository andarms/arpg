using rlImGui_cs;
using ImGuiNET;
namespace Arpg.Editor;

public static class Window
{
  static GameEditor editor;

  public static void Initialize()
  {
    InitWindow(1280, 720, "Editor");
    SetTargetFPS(60);
    editor = new GameEditor();
    rlImGui.Setup(true);

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
    rlImGui.Shutdown();
  }

  private static void Update()
  {
    editor.Update();
  }

  private static void Draw()
  {
    ClearBackground(Color.Black);
    BeginDrawing();
    rlImGui.Begin();
    editor.Draw();

    rlImGui.End();
    EndDrawing();
  }
}