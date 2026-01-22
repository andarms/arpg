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

    if (ImGui.BeginMainMenuBar())
    {
      if (ImGui.BeginMenu("File"))
      {
        if (ImGui.MenuItem("New"))
        {
          // Handle new file action
        }
        if (ImGui.MenuItem("Open"))
        {
          // Handle open file action
        }
        if (ImGui.MenuItem("Save"))
        {
          // Handle save file action
        }
        ImGui.Separator();
        if (ImGui.MenuItem("Exit"))
        {
          CloseWindow();
        }
        ImGui.EndMenu();
      }

      if (ImGui.BeginMenu("Edit"))
      {
        if (ImGui.MenuItem("Undo"))
        {
          // Handle undo action
        }
        if (ImGui.MenuItem("Redo"))
        {
          // Handle redo action
        }
        ImGui.EndMenu();
      }

      ImGui.EndMainMenuBar();
    }
    editor.Draw();
    rlImGui.End();
    EndDrawing();
  }
}