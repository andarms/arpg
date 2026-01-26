using Arpg.Engine.Assets;
using Arpg.Game.Gom;

namespace Arpg.Game.GameObjects;

public class Player : GameObject
{
  public Player() : base()
  {
    var animatedSprite = new AnimatedSprite()
    {
      Texture = AssetsManager.Textures["Sprites/base"],
      Anchor = new Vector2(8, 16)
    };
    animatedSprite.AddAnimation("WalkDown", new Animation
    {
      Frames = [new Rectangle(0, 16, 16, 16), new Rectangle(16, 16, 16, 16)],
      FrameDuration = 0.2f
    });
    components.Add(animatedSprite);
    animatedSprite.Play("WalkDown");
    components.Add(new CameraFollowComponent());
    components.Add(new FacingDirection());

    States.Register(new PlayerMoving());
    States.SetInitial<PlayerMoving>();
  }
}

public class PlayerMoving : GameObjectState
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
      FacingDirection? facing = Owner.Get<FacingDirection>();
      facing?.SetDirection(input);
    }
  }
}

public class CameraFollowComponent : GameObjectComponent
{
  public override void Update(float dt)
  {
    Game.Viewport.UpdateTarget(Owner.Position);
  }
}

public enum Direction
{
  Up,
  Down,
  Left,
  Right
}

public class FacingDirection : GameObjectComponent
{
  public Direction Direction { get; set; } = Direction.Down;

  public void SetDirection(Vector2 movement)
  {
    if (movement.X > 0)
    {
      Direction = Direction.Right;
    }
    else if (movement.X < 0)
    {
      Direction = Direction.Left;
    }
    else if (movement.Y > 0)
    {
      Direction = Direction.Down;
    }
    else if (movement.Y < 0)
    {
      Direction = Direction.Up;
    }
  }
}