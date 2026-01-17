using Arpg.Game.Assets;
using Arpg.Game.Gom;

namespace Arpg.Game.GameObjects;

public class Player : GameObject
{
  public Player() : base()
  {
    Sprite = new() { Texture = AssetsManager.Textures["TinyDungeon"], Source = new Rectangle(16, 112, 16, 16) };
    components.Add(new CameraFollowComponent());
    components.Add(new PlayerInput());
  }
}



public class CameraFollowComponent : GameObjectComponent
{
  public override void Update(float dt)
  {
    Game.Viewport.UpdateTarget(Owner.Position);
  }
}

public class PlayerInput : GameObjectComponent
{
  private const float Speed = 100f;

  public override void Update(float dt)
  {
    Vector2 input = Vector2.Zero;
    if (IsKeyDown(KeyboardKey.Up)) { input.Y -= 1; }
    if (IsKeyDown(KeyboardKey.Down)) { input.Y += 1; }
    if (IsKeyDown(KeyboardKey.Left)) { input.X -= 1; }
    if (IsKeyDown(KeyboardKey.Right)) { input.X += 1; }

    if (input.Length() > 0)
    {
      input = Vector2.Normalize(input);
      Owner.Position += input * Speed * dt;
    }
  }
}