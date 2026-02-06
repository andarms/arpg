using Arpg.Engine;

namespace Arpg.Editor.GameConsole;

public class InputField
{
  Vector2 position = new(Constants.Padding, GetScreenHeight() - Constants.Padding4);
  Rectangle Bound = new(Constants.Padding, GetScreenHeight() - Constants.Padding4, GetScreenWidth() - Constants.Padding2, 40);

  string text = "Hello world";

  private float cursorTimer = 0f;

  public Action<string>? OnSubmit;

  public void Update(float dt)
  {
    cursorTimer += dt;

    // Handle backspace
    if (IsKeyPressed(KeyboardKey.Backspace) && text.Length > 0)
    {
      text = text[..^1];
    }

    // Handle enter key
    if (IsKeyPressed(KeyboardKey.Enter) && text.Length > 0)
    {
      OnSubmit?.Invoke(text);
      text = "";
    }

    // Handle character input
    int key = GetCharPressed();
    while (key > 0)
    {
      if (key >= 32 && key <= 126) // Printable ASCII range
      {
        text += (char)key;
      }
      key = GetCharPressed(); // Get next character if any
    }
  }


  public void Draw()
  {
    DrawRectangleRec(Bound, new Color(30, 30, 30, 180));
    DrawTextEx(Constants.DefaultFont, "$", new Vector2(position.X + 10, position.Y), 32, 2, Color.White);
    DrawTextEx(Constants.DefaultFont, text, new Vector2(position.X + 40, position.Y), 32, 2, Color.White);

    // Draw blinking cursor
    if ((cursorTimer % 1.0f) < 0.5f) // Blink every 0.5 seconds
    {
      Vector2 textSize = MeasureTextEx(Constants.DefaultFont, text, 32, 2);
      DrawTextEx(Constants.DefaultFont, "_", new Vector2(position.X + 40 + textSize.X, position.Y), 32, 2, Color.White);
    }
  }
}