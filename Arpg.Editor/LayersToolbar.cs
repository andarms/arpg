namespace Arpg.Editor;

public class LayersToolbar
{
  Vector2 Position = new(GetScreenWidth() - 416 - Constants.Padding, Constants.Padding * 2 + Constants.Padding);
  public static Vector2 Size = new(200, 32);

  Rectangle Button1;
  Rectangle Button2;
  Rectangle Button3;
  Rectangle Button4;
  Rectangle Button5;

  readonly Rectangle[] buttons = [];

  readonly KeyboardKey[] layerKeys =
  [
      KeyboardKey.One,
      KeyboardKey.Two,
      KeyboardKey.Three,
      KeyboardKey.Four,
      KeyboardKey.Five
  ];

  public LayersToolbar()
  {
    Button1 = new(Position.X, Position.Y, 32, 32);
    Button2 = new(Position.X + 40, Position.Y, 32, 32);
    Button3 = new(Position.X + 80, Position.Y, 32, 32);
    Button4 = new(Position.X + 120, Position.Y, 32, 32);
    Button5 = new(Position.X + 160, Position.Y, 32, 32);

    buttons = [
        Button1,
        Button2,
        Button3,
        Button4,
        Button5
    ];
  }



  public void Draw()
  {
    // DrawRectangleV(Position, Size, Color.DarkGray);
    DrawRectangleRec(Button1, GetButtonColor(0));
    DrawTextEx(Constants.DefaultFont, "1", new Vector2(Button1.X + 10, Button1.Y), 32, 0, Color.Black);
    DrawRectangleRec(Button2, GetButtonColor(1));
    DrawTextEx(Constants.DefaultFont, "2", new Vector2(Button2.X + 10, Button2.Y), 32, 0, Color.Black);
    DrawRectangleRec(Button3, GetButtonColor(2));
    DrawTextEx(Constants.DefaultFont, "3", new Vector2(Button3.X + 10, Button3.Y), 32, 0, Color.Black);
    DrawRectangleRec(Button4, GetButtonColor(3));
    DrawTextEx(Constants.DefaultFont, "C", new Vector2(Button4.X + 10, Button4.Y), 32, 0, Color.Black);
    DrawRectangleRec(Button5, GetButtonColor(4));
    DrawTextEx(Constants.DefaultFont, "GO", new Vector2(Button5.X + 2, Button5.Y), 32, 0, Color.Black);
  }


  public Color GetButtonColor(int layer)
  {
    return GameEditorViewModel.SelectedLayer == layer ? Color.White : Color.Gray;
  }

  public void Update()
  {
    if (IsMouseButtonPressed(MouseButton.Left))
    {
      Vector2 mousePos = GetMousePosition();

      for (int i = 0; i < buttons.Length; i++)
      {
        if (CheckCollisionPointRec(mousePos, buttons[i]))
        {
          GameEditorViewModel.SelectedLayer = i;
          GameEditorViewModel.ShowGrid = true;
          break;
        }
      }
    }

    for (int i = 0; i < layerKeys.Length; i++)
    {
      if (IsKeyPressed(layerKeys[i]))
      {
        GameEditorViewModel.SelectedLayer = i;
        break;
      }
    }
  }
}
