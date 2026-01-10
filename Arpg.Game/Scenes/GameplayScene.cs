using Arpg.Game.Gom;

namespace Arpg.Game.Scenes;

public class GameplayScene : Scene
{
  public override void Initialize()
  {
    base.Initialize();
    Game.CurrentScene.Add(new Player(), [GameObjectGroup.Player]);
    SpawnRandomWalls();
  }

  private static void SpawnRandomWalls()
  {
    for (int i = 0; i < 10; i++)
    {
      int x = Game.Rng.Next(0, 800);
      int y = Game.Rng.Next(0, 600);
      var wall = new Wall
      {
        Position = new Vector2(x, y)
      };
      Game.CurrentScene.Add(wall, [GameObjectGroup.Obstacle]);
    }
  }

  public override void Terminate()
  {
  }
}