namespace Arpg.Game.Scenes;

public class ScenesController
{
  public Scene? CurrentScene { get; private set; } = null;

  readonly Dictionary<Type, Scene> scenes = [];

  public void Register<T>(T scene) where T : Scene
  {
    scenes[typeof(T)] = scene;
  }

  public void SwitchTo<T>() where T : Scene
  {
    if (!scenes.TryGetValue(typeof(T), out var existingScene))
    {
      return;
    }
    CurrentScene?.Terminate();
    CurrentScene = existingScene;
    CurrentScene.Initialize();
    return;
  }
}
