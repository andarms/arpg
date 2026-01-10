using Arpg.Game.Gom;
using Arpg.Game.Scenes;

namespace Arpg.Game;

public static class Game
{
  public static ScenesController Scenes { get; } = new();

  public static void Initialize()
  {
    Scenes.Register(new GameplayScene());
    Scenes.SwitchTo<GameplayScene>();
    Scenes.CurrentScene?.Initialize();
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

  public static void Add(GameObject gameObject)
  {
    Scenes.CurrentScene?.Add(gameObject);
  }

  public static void Remove(GameObject gameObject)
  {
    Scenes.CurrentScene?.Remove(gameObject);
  }
}
