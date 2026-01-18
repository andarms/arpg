public class LayersToolbar
{
  Vector2 Position = new(GetScreenWidth() - MiniMapPanel.Size.X - Settings.Padding, Settings.Padding + MiniMapPanel.Size.Y + Settings.Padding);
  public static Vector2 Size = new(200, 32);

  Rectangle Button1;
  Rectangle Button2;
  Rectangle Button3;
  Rectangle Button4;
  Rectangle Button5;


  public LayersToolbar()
  {
    Button1 = new(Position.X, Position.Y, 32, 32);
    Button2 = new(Position.X + 40, Position.Y, 32, 32);
    Button3 = new(Position.X + 80, Position.Y, 32, 32);
    Button4 = new(Position.X + 120, Position.Y, 32, 32);
    Button5 = new(Position.X + 160, Position.Y, 32, 32);
  }

  public void Draw()
  {
    // DrawRectangleV(Position, Size, Color.DarkGray);
    DrawRectangleRec(Button1, Color.White);
    DrawTextEx(Settings.DefaultFont, "1", new Vector2(Button1.X + 10, Button1.Y), 32, 0, Color.Black);
    DrawRectangleRec(Button2, Color.Gray);
    DrawTextEx(Settings.DefaultFont, "2", new Vector2(Button2.X + 10, Button2.Y), 32, 0, Color.Black);
    DrawRectangleRec(Button3, Color.Gray);
    DrawTextEx(Settings.DefaultFont, "3", new Vector2(Button3.X + 10, Button3.Y), 32, 0, Color.Black);
    DrawRectangleRec(Button4, Color.Gray);
    DrawTextEx(Settings.DefaultFont, "C", new Vector2(Button4.X + 10, Button4.Y), 32, 0, Color.Black);
    DrawRectangleRec(Button5, Color.Gray);
    DrawTextEx(Settings.DefaultFont, "GO", new Vector2(Button5.X + 2, Button5.Y), 32, 0, Color.Black);
  }
}
