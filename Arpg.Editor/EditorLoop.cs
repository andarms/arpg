using Arpg.Editor.GameConsole;
using Arpg.Editor.RoomsEditor;
using Arpg.Engine;
using Arpg.Engine.Scenes;

namespace Arpg.Editor;

public class EditorLoop : ILoop
{
  public event Action? OnSwitchRequested;


  public void Initialize()
  {
    ScenesController.AddScene(new RoomEditorScene());
    ScenesController.AddScene(new ConsoleScene());
    ScenesController.AddScene(new RoomsSelectionScene());
    ScenesController.AddScene(new NewRoomScene());
    ScenesController.AddScene(new EditorMenuScene());
    ScenesController.SwitchTo<EditorMenuScene>();
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