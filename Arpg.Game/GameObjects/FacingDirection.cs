using Arpg.Game.Gom;

namespace Arpg.Game.GameObjects;

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