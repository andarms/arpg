using Arpg.Engine;
using Arpg.Engine.Assets;

namespace Arpg.Game.Core;

public class GameLoop : ILoop
{
  public event Action? OnSwitchRequested;

  public GameLoop()
  {
  }

  public void Initialize()
  {
    AssetsManager.SetAssetBasePath(Engine.Settings.Get<string>("AssetsPath"));
    AssetsManager.LoadAssets();
    Engine.Tilemaps.TilemapService.Initialize();
    Game.Initialize();
  }

  public void Update(float deltaTime)
  {
    // Check for mode switch (F11 to editor)
    if (IsKeyPressed(KeyboardKey.F11))
    {
#if DEBUG
      OnSwitchRequested?.Invoke();
      return;
#endif
    }

    Game.Update(deltaTime);
  }

  public void Draw()
  {
    ClearBackground(Color.Black);
    BeginMode2D(Game.Viewport.Camera);
    Game.Draw();
    EndMode2D();

    // Game.DrawUI();
    if (Game.DebugMode)
    {
      DrawFPS(10, 10);
    }
  }

  public void OnExit()
  {
    AssetsManager.UnloadAssets();
  }
}