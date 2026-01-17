using Arpg.Game.GameObjects;
using Arpg.Game.Gom;

namespace Arpg.Game.Rooms;

public class Gameplay : Room
{
  public Gameplay() : base()
  {
    Add(new Player(), [GameObjectGroup.Player]);
    Add(new Chest() { Position = new Vector2(100, 100) }, [GameObjectGroup.Obstacle]);
  }


  public override void Terminate()
  {
  }
}