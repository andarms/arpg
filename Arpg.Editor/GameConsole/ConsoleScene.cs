using Arpg.Engine.Scenes;

namespace Arpg.Editor.GameConsole;

public class ConsoleScene : Scene
{
  readonly InputField inputField = new();

  public override void Update(float dt)
  {
    base.Update(dt);
    if (IsKeyPressed(Constants.ConsoleToggleKey))
    {
      ScenesController.PopScene();
    }
    inputField.Update(dt);
  }

  public override void Draw()
  {
    base.Draw();
    DrawRectangle(0, 0, GetScreenWidth(), GetScreenHeight(), new Color(0, 0, 0, 220));
    // DrawTextEx(Constants.DefaultFont, "EDITOR MODE\n\nF5 - Switch to Game Mode\n; - Toggle Overlay", new Vector2(20, 20), 24, 2, Color.White);

    inputField.Draw();
  }
}