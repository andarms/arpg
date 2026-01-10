using Arpg.Game.Gom;
using Arpg.Game.Scenes;

namespace Arpg.Game;

public static class Game
{
  public static ScenesController Scenes { get; } = new();
  public static RandomNumberGenerator Rng { get; } = new();
  public static Scene CurrentScene => Scenes.CurrentScene ?? throw new InvalidOperationException("No current scene");

  public static void Initialize()
  {
    Scenes.Register(new GameplayScene());
    Scenes.SwitchTo<GameplayScene>();
  }

  public static void Update(float dt)
  {
    Scenes.CurrentScene?.Update(dt);
  }
  public static void Draw()
  {
    Scenes.CurrentScene?.Draw();
  }

  public static void Terminate()
  {
    Scenes.CurrentScene?.Terminate();
  }
}
