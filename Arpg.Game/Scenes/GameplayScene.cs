using Arpg.Game.Gom;

namespace Arpg.Game.Scenes;

public class GameplayScene : Scene
{
  public override void Initialize()
  {
    base.Initialize();
    Game.Add(new Player());
  }

  public override void Terminate()
  {
  }
}