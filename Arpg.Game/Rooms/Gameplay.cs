using Arpg.Game.Gom;

namespace Arpg.Game.Rooms;

public class Gameplay : Room
{
  public override void Initialize()
  {
    base.Initialize();
    Game.ActiveRoom.Add(new Player(), [GameObjectGroup.Player]);
  }


  public override void Terminate()
  {
  }
}