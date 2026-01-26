using Arpg.Game.Gom;

namespace Arpg.Game.GameObjects;

public class CameraFollowComponent : GameObjectComponent
{
  public override void Update(float dt)
  {
    Game.Viewport.UpdateTarget(Owner.Position);
  }
}