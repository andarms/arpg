using Arpg.Game.GameObjects;
using Arpg.Game.Gom;

namespace Arpg.Game.Rooms;

public class Gameplay : Room
{
  public Gameplay() : base()
  {
    Add(new Player() { Position = new Vector2(50, 50) }, [GameObjectGroup.Player]);
    Add(new Chest() { Position = new Vector2(100, 100) }, [GameObjectGroup.Obstacle]);
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    if (IsKeyPressed(KeyboardKey.F1))
    {
      Game.DebugMode = !Game.DebugMode;
    }
  }
}