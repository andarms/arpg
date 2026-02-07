using Arpg.Editor.Components;
using Arpg.Engine.Scenes;
using System.IO;

namespace Arpg.Editor;

public class NewRoomScene : Scene
{
  private const int ITEM_HEIGHT = 50;
  private const int PADDING = 20;
  private const int HEADER_HEIGHT = 50;
  private const int INPUT_WIDTH = 600;

  private string roomName = "";
  private string roomWidth = "";
  private string roomHeight = "";
  private int selectedField = 0; // 0=name, 1=width, 2=height
  private readonly Color backgroundColor = new(0, 0, 0, 240);
  private readonly Color inputBackgroundColor = new(30, 30, 30, 180);
  private readonly Color selectedInputColor = new(64, 128, 255, 200);
  private readonly Color inputBorderColor = new(100, 100, 100, 255);

  public override void Update(float dt)
  {
    // Handle field navigation
    if (IsKeyPressed(KeyboardKey.Tab) || IsKeyPressed(KeyboardKey.Down))
    {
      selectedField = (selectedField + 1) % 3;
    }
    else if (IsKeyPressed(KeyboardKey.Up))
    {
      selectedField = (selectedField - 1 + 3) % 3;
    }

    // Handle text input for the selected field
    HandleTextInput();

    // Handle special keys
    if (IsKeyPressed(KeyboardKey.Enter))
    {
      CreateRoom();
    }

    if (IsKeyPressed(KeyboardKey.Escape))
    {
      ScenesController.PopScene();
    }
  }

  private void HandleTextInput()
  {
    int key = GetCharPressed();
    while (key > 0)
    {
      // Add character to the selected field
      switch (selectedField)
      {
        case 0: // Room name
          if (char.IsLetterOrDigit((char)key) || char.IsWhiteSpace((char)key) || char.IsPunctuation((char)key))
          {
            roomName += (char)key;
          }
          break;
        case 1: // Width
          if (char.IsDigit((char)key))
          {
            roomWidth += (char)key;
          }
          break;
        case 2: // Height
          if (char.IsDigit((char)key))
          {
            roomHeight += (char)key;
          }
          break;
      }
      key = GetCharPressed();
    }

    // Handle backspace
    if (IsKeyPressed(KeyboardKey.Backspace))
    {
      switch (selectedField)
      {
        case 0:
          if (roomName.Length > 0) roomName = roomName[..^1];
          break;
        case 1:
          if (roomWidth.Length > 0) roomWidth = roomWidth[..^1];
          break;
        case 2:
          if (roomHeight.Length > 0) roomHeight = roomHeight[..^1];
          break;
      }
    }
  }

  private void CreateRoom()
  {
    // Validate inputs
    if (string.IsNullOrWhiteSpace(roomName))
    {
      // TODO: Show error message
      return;
    }

    if (!int.TryParse(roomWidth, out int width) || width <= 0)
    {
      // TODO: Show error message
      return;
    }

    if (!int.TryParse(roomHeight, out int height) || height <= 0)
    {
      // TODO: Show error message
      return;
    }

    try
    {
      // Split room name to handle folder structure
      var pathParts = roomName.Split('/', StringSplitOptions.RemoveEmptyEntries);
      string baseRoomsPath = "C:\\Users\\andar\\apps\\hamaka_studio\\arpg\\Arpg.Game\\Assets\\Rooms";

      // Build the full directory path
      string directoryPath = baseRoomsPath;
      for (int i = 0; i < pathParts.Length - 1; i++)
      {
        directoryPath = Path.Combine(directoryPath, pathParts[i]);
      }

      // Create the directory if it doesn't exist
      if (!Directory.Exists(directoryPath))
      {
        Directory.CreateDirectory(directoryPath);
      }

      // Create the room file path
      string roomFileName = pathParts.Length > 0 ? pathParts[^1] + ".room" : roomName + ".room";
      string fullRoomPath = Path.Combine(directoryPath, roomFileName);

      // Create a new tilemap with specified dimensions
      // Using a default tileset path - you may want to make this configurable
      string defaultTilesetPath = "Textures/tileset.png";
      GameEditorViewModel.CreateTilemap(width, height, defaultTilesetPath);

      // Set the filepath and save
      if (GameEditorViewModel.Tilemap != null)
      {
        GameEditorViewModel.Tilemap.FilePath = fullRoomPath;
        GameEditorViewModel.Tilemap.Save();
        Console.WriteLine($"Created room: {roomName} ({width}x{height}) at {fullRoomPath}");
        ScenesController.SwitchTo<RoomEditorScene>();
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Failed to create room: {ex.Message}");
      // TODO: Show error message to user
      return;
    }

    // Close the scene
    ScenesController.PopScene();
  }

  public override void Draw()
  {
    base.Draw();
    DrawRectangle(0, 0, GetScreenWidth(), GetScreenHeight(), backgroundColor);

    int screenWidth = GetScreenWidth();
    int screenHeight = GetScreenHeight();

    // Calculate centered position for the form
    int formX = (screenWidth - INPUT_WIDTH - PADDING * 2) / 2;
    int startY = (screenHeight - (HEADER_HEIGHT + ITEM_HEIGHT * 3 + PADDING * 4)) / 2;

    // Header
    DrawTextEx(Constants.DefaultFont, "Create New Room",
               new Vector2(formX, startY), 24, 1, Color.White);

    // Draw input fields
    string[] labels = { "Room Name:", "Width:", "Height:" };
    string[] values = { roomName, roomWidth, roomHeight };

    for (int i = 0; i < 3; i++)
    {
      int fieldY = startY + HEADER_HEIGHT + (i * (ITEM_HEIGHT + 48));
      bool isSelected = i == selectedField;

      // Draw label
      DrawTextEx(Constants.DefaultFont, labels[i],
                 new Vector2(formX, fieldY), 18, 1, Color.LightGray);

      // Draw input background
      Rectangle inputRect = new(formX, fieldY + 25, INPUT_WIDTH, ITEM_HEIGHT);
      Color bgColor = isSelected ? selectedInputColor : inputBackgroundColor;
      DrawRectangle((int)inputRect.X, (int)inputRect.Y, (int)inputRect.Width, (int)inputRect.Height, bgColor);
      DrawRectangleLines((int)inputRect.X, (int)inputRect.Y, (int)inputRect.Width, (int)inputRect.Height, inputBorderColor);

      // Draw input text
      string displayText = values[i];
      if (isSelected && (int)(GetTime() * 2) % 2 == 0) // Blinking cursor
      {
        displayText += "_";
      }

      DrawTextEx(Constants.DefaultFont, displayText,
                 new Vector2(inputRect.X + 15, inputRect.Y + 15), 20, 1, Color.White);
    }

    // Draw instructions
    int instructionsY = startY + HEADER_HEIGHT + (3 * (ITEM_HEIGHT + 10)) + 20;
    DrawTextEx(Constants.DefaultFont, "Tab/↑↓ to navigate fields, Enter to create, Esc to cancel",
               new Vector2(formX, instructionsY), 14, 1, Color.Gray);

    // Draw validation hints
    int hintsY = instructionsY + 25;
    if (selectedField == 1 || selectedField == 2)
    {
      DrawTextEx(Constants.DefaultFont, "Enter numbers only",
                 new Vector2(formX, hintsY), 12, 1, Color.Yellow);
    }
  }
}