using Arpg.Engine;

namespace Arpg.Editor;

public class EditorLoop : ILoop
{
  private GameEditor? editor;

  public event Action? OnSwitchRequested;
  Color backgroundColor = Color.Black;


  bool showOverlay = false;
  public void Initialize()
  {
    editor = new GameEditor();
  }

  public void Update(float deltaTime)
  {
    // Check for mode switch (F5 to game)
    if (IsKeyPressed(KeyboardKey.F5))
    {
      OnSwitchRequested?.Invoke();
      return;
    }

    if (IsKeyPressed(KeyboardKey.Semicolon))
    {
      showOverlay = !showOverlay;
    }

    editor?.Update();
  }

  public void Draw()
  {
    ClearBackground(backgroundColor);
    editor?.Draw();
    if (showOverlay)
    {
      DrawRectangle(0, 0, GetScreenWidth(), GetScreenHeight(), new Color(0, 0, 0, 220));
      DrawTextEx(Constants.DefaultFont, "EDITOR MODE\n\nF5 - Switch to Game Mode\n; - Toggle Overlay", new Vector2(20, 20), 24, 2, Color.White);
    }
  }

  public void OnExit()
  {
  }
}