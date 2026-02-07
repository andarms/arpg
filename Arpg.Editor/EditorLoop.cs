using Arpg.Editor.GameConsole;
using Arpg.Engine;
using Arpg.Engine.Scenes;

namespace Arpg.Editor;

public class EditorLoop : ILoop
{
  public event Action? OnSwitchRequested;


  public void Initialize()
  {
    // Create and add the main editor scene
    var editorScene = new GameEditorScene();
    ScenesController.AddScene(editorScene);
    ScenesController.AddScene(new ConsoleScene());
    ScenesController.AddScene(new RoomsSelectionScene());
    ScenesController.SwitchTo<GameEditorScene>();
  }

  public void Update(float deltaTime)
  {
    // Check for mode switch (F5 to game)
    if (IsKeyPressed(KeyboardKey.F5))
    {
      OnSwitchRequested?.Invoke();
      return;
    }

    // Update the scene manager
    ScenesController.Update(deltaTime);
  }

  public void Draw()
  {
    // Use scene manager to handle drawing
    ScenesController.Draw();
  }

  public void OnExit()
  {
    // Clean up scenes when exiting
    ScenesController.ClearAllScenes();
  }
}