using Arpg.Editor.Components;
using Arpg.Engine.Scenes;

namespace Arpg.Editor;

public class EditorMenuScene : Scene
{
  private SelectionList<string> menuOptions = null!;
  private const int ITEM_HEIGHT = 40;
  private const int PADDING = 20;
  private const int HEADER_HEIGHT = 50;
  private readonly Color backgroundColor = new(0, 0, 0, 240);

  public EditorMenuScene()
  {
    LoadMenuOptions();
  }

  private void LoadMenuOptions()
  {
    var options = new List<string>
    {
      "Create New Room",
      "Load Rooms"
    };

    menuOptions = new SelectionList<string>(options);
    if (options.Count > 0)
    {
      menuOptions.SelectNext(); // Select first option
    }
  }

  public override void Update(float dt)
  {
    menuOptions.Update(dt);

    if (IsKeyPressed(KeyboardKey.Enter) && menuOptions.SelectedIndex >= 0)
    {
      OnMenuSelection(menuOptions.SelectedIndex);
    }

    if (IsKeyPressed(KeyboardKey.Escape))
    {
      ScenesController.PopScene();
    }
  }

  public override void Draw()
  {
    base.Draw();

    int screenWidth = GetScreenWidth();
    int screenHeight = GetScreenHeight();

    // Draw subtle background
    DrawRectangle(0, 0, screenWidth, screenHeight, backgroundColor);

    // Header
    DrawTextEx(Constants.DefaultFont, "Editor Menu",
               new Vector2(PADDING, PADDING), 28, 1, Color.White);

    // Draw menu options using ListSelection component
    int listStartY = PADDING + HEADER_HEIGHT;
    int listWidth = screenWidth - (PADDING * 2);
    menuOptions.Draw(PADDING, listStartY, listWidth, ITEM_HEIGHT, 22);

    // Draw instructions at bottom
    int instructionsY = screenHeight - 40;
    DrawTextEx(Constants.DefaultFont, "Use ↑↓ to navigate, Enter to select, Esc to go back",
               new Vector2(PADDING, instructionsY), 16, 1, Color.LightGray);
  }

  private void OnMenuSelection(int selectedIndex)
  {
    switch (selectedIndex)
    {
      case 0: // Create New Room
        ScenesController.PushScene<NewRoomScene>();
        break;
      case 1: // Load Rooms
        ScenesController.PushScene<RoomsSelectionScene>();
        break;
    }
  }
}