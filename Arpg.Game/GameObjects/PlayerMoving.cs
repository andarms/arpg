using Arpg.Engine.Gom;

namespace Arpg.Game.GameObjects;

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
      Vector2 movement = input * Speed * dt;
      Vector2 targetPosition = Owner.Position + movement;

      // Use collision-aware movement
      var collider = Owner.Get<Collider>();
      if (collider != null)
      {
        Owner.Position = Game.Collisions.MoveAndCollide(
          Owner.Position,
          targetPosition,
          collider,
          GameObjectGroup.Player // Exclude player group from collision checks
        );
      }
      else
      {
        Owner.Position = targetPosition; // Fallback if no collider
      }
      FacingDirection? facing = Owner.Get<FacingDirection>();
      facing?.SetDirection(input);
      AnimatedSprite? animatedSprite = Owner.Get<AnimatedSprite>();
      if (animatedSprite != null)
      {
        switch (facing?.Direction)
        {
          case Direction.Up:
            animatedSprite.Play("WalkUp");
            break;
          case Direction.Down:
            animatedSprite.Play("WalkDown");
            break;
          case Direction.Left:
            animatedSprite.Play("WalkLeft");
            break;
          case Direction.Right:
            animatedSprite.Play("WalkRight");
            break;
        }
      }
    }
  }
}
