namespace Arpg.Game;

public class Window
{
  public void Initialize()
  {
    InitWindow(Settings.WindowWidth, Settings.WindowHeight, "ARPG Game");
    SetTargetFPS(60);
    Game.Initialize();
  }

  public void Run()
  {
    Initialize();
    Loop();
    CloseWindow();
  }

  private static void Loop()
  {
    while (!WindowShouldClose())
    {
      BeginDrawing();
      float dt = GetFrameTime();
      Game.Update(dt);
      Game.Draw();
      EndDrawing();
    }
  }
}
