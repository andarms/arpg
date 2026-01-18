public class MiniMapPanel
{
  Vector2 Position = new(GetScreenWidth() - 432 - Settings.Padding, Settings.Padding);
  public static Vector2 Size = new(432, 288);

  public void Draw()
  {
    DrawRectangleV(Position, Size, Color.DarkGray);
  }
}
