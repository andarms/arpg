using Arpg.Game.Assets;
using Arpg.Game.Gom;

namespace Arpg.Game.GameObjects;

public class Player : GameObject
{
  public Player() : base()
  {
    Sprite = new() { Texture = AssetsManager.Textures["TinyDungeon"], Source = new Rectangle(16, 112, 16, 16) };
    components.Add(new CameraFollowComponent());
  }
}



public class CameraFollowComponent : GameObjectComponent
{
  public override void Update(float dt)
  {
    Game.Viewport.UpdateTarget(Owner.Position);
  }
}