using Arpg.Game.Gom;

namespace Arpg.Game;

public class Wall : GameObject
{
  Vector2 size = new(16, 16);
  public bool CloseToPlayer { get; set; } = false;

  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, size, CloseToPlayer ? Color.Red : Color.Gray);
  }
}
