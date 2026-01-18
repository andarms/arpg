namespace Arpg.Editor;

public class ToolToolbar
{
  Vector2 Position = new Vector2(GetScreenWidth() - Settings.Padding - MiniMapPanel.Size.X, Settings.Padding * 3 + MiniMapPanel.Size.Y + LayersToolbar.Size.Y);
  public static Vector2 Size = new Vector2(240, 32);
  Rectangle Button1;
  Rectangle Button2;
  Rectangle Button3;

  Rectangle[] buttons = [];

  readonly KeyboardKey[] toolShortcuts =
  [
      KeyboardKey.J,
      KeyboardKey.K,
      KeyboardKey.L
  ];


  public ToolToolbar()
  {
    Button1 = new(Position.X, Position.Y, 32, 32);
    Button2 = new(Position.X + 40, Position.Y, 32, 32);
    Button3 = new(Position.X + 80, Position.Y, 32, 32);

    buttons = [
        Button1,
        Button2,
        Button3
    ];
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
          GameEditorViewModel.SelectedTool = i;
          break;
        }
      }
    }

    if (IsKeyPressed(toolShortcuts[0]))
    {
      GameEditorViewModel.SelectedTool = 0;
    }
    else if (IsKeyPressed(toolShortcuts[1]))
    {
      GameEditorViewModel.SelectedTool = 1;
    }
    else if (IsKeyPressed(toolShortcuts[2]))
    {
      GameEditorViewModel.SelectedTool = 2;
    }
  }

  public void Draw()
  {
    DrawButton();
  }

  public void DrawButton()
  {
    Rectangle source1 = new(160, 112, 16, 16);
    DrawTexturePro(Settings.CursorTexture, source1, Button1, Vector2.Zero, 0.0f, GetButtonColor(0));

    Rectangle source2 = new(208, 112, 16, 16);
    DrawTexturePro(Settings.CursorTexture, source2, Button2, Vector2.Zero, 0.0f, GetButtonColor(1));

    Rectangle source3 = new(144, 112, 16, 16);
    DrawTexturePro(Settings.CursorTexture, source3, Button3, Vector2.Zero, 0.0f, GetButtonColor(2));
  }

  public Color GetButtonColor(int tool)
  {
    return GameEditorViewModel.SelectedTool == tool ? Color.White : Color.Gray;
  }
}