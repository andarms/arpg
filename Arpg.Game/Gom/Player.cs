namespace Arpg.Game.Gom;

public class Player : GameObject
{
  Vector2 size = new(16, 16);
  float speed = 100f;

  public override void Update(float dt)
  {
    base.Update(dt);
    Vector2 dir = GetInputDirection();
    Vector2 movement = Position + dir * speed * dt;
    var objs = Game.CurrentScene.Get(GameObjectGroup.Obstacle);
    Position = movement;
    // foreach (var obj in objs)
    // {
    //   Rectangle playerRect = new Rectangle((int)movement.X, (int)movement.Y, (int)size.X, (int)size.Y);
    //   Rectangle obstacleRect = new Rectangle((int)obj.Position.X, (int)obj.Position.Y, 32, 32);
    //   if (CheckCollisionRecs(playerRect, obstacleRect))
    //   {
    //     return;
    //   }
    // }
  }

  Vector2 GetInputDirection()
  {
    Vector2 dir = Vector2.Zero;
    if (IsKeyDown(KeyboardKey.Up)) dir.Y -= 1;
    if (IsKeyDown(KeyboardKey.Down)) dir.Y += 1;
    if (IsKeyDown(KeyboardKey.Left)) dir.X -= 1;
    if (IsKeyDown(KeyboardKey.Right)) dir.X += 1;
    if (dir != Vector2.Zero) dir = Vector2.Normalize(dir);
    return dir;
  }

  public override void Draw()
  {
    base.Draw();
    DrawRectangleV(Position, size, Color.Blue);
  }
}
