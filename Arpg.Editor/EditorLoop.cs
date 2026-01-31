using Arpg.Engine;
using rlImGui_cs;
using ImGuiNET;

namespace Arpg.Editor;

public class EditorLoop : ILoop
{
  private GameEditor? editor;

  public event Action? OnSwitchRequested;
  Color backgroundColor = Color.Gray;

  public void Initialize()
  {
    editor = new GameEditor();
    rlImGui.Setup(true);
  }

  public void Update(float deltaTime)
  {
    // Check for mode switch (F5 to game)
    if (IsKeyPressed(KeyboardKey.F5))
    {
      OnSwitchRequested?.Invoke();
      return;
    }

    editor?.Update();
  }

  public void Draw()
  {
    ClearBackground(backgroundColor);
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
          OnSwitchRequested?.Invoke();
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

    editor?.Draw();
    rlImGui.End();
  }

  public void OnExit()
  {
    rlImGui.Shutdown();
  }
}