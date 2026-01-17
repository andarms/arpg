using Arpg.Game.Assets;

namespace Arpg.Game.Core;

public class Window
{
  public static void Initialize()
  {
    InitWindow(Settings.WindowWidth, Settings.WindowHeight, "ARPG Game");
    SetTargetFPS(60);
    AssetsManager.LoadAssets();
    Game.Initialize();
  }

  public static void Run()
  {
    Initialize();
    Loop();
    AssetsManager.UnloadAssets();
    CloseWindow();
  }

  private static void Loop()
  {
    while (!WindowShouldClose())
    {
      float dt = GetFrameTime();
      Game.Update(dt);
      BeginDrawing();
      BeginMode2D(Game.Viewport.Camera);
      Game.Draw();
      EndMode2D();
      // Game.DrawUI();
      EndDrawing();
    }
  }
}
