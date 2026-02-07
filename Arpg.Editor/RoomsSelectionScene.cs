using Arpg.Editor.Components;
using Arpg.Editor.RoomsEditor;
using Arpg.Editor.Utils;
using Arpg.Engine.Scenes;

namespace Arpg.Editor;

public class RoomsService
{
  public string[] GetAllRooms()
  {
    return LoadRoomsFromProject();
  }

  public string? GetRoomByIndex(int index)
  {
    var rooms = GetAllRooms();
    if (index >= 1 && index <= rooms.Length)
    {
      return rooms[index - 1]; // Convert from 1-based to 0-based indexing
    }
    return null;
  }

  public string[] GetNumberedRoomsList()
  {
    var rooms = GetAllRooms();
    return [.. rooms.Select((room, index) => $"{index + 1}. {room}")];
  }

  private string[] LoadRoomsFromProject()
  {
    try
    {
      string fullPath = FilePathService.GetRoomsDirectory();

      if (!Directory.Exists(fullPath))
      {
        return [$"Rooms directory not found: {fullPath}"];
      }

      var roomFiles = Directory.GetFiles(fullPath, "*.room", SearchOption.AllDirectories);

      // Return relative paths from the Rooms directory
      return [.. roomFiles.Select(file => Path.GetRelativePath(fullPath, file))];
    }
    catch (Exception ex)
    {
      return [$"Error loading rooms: {ex.Message}"];
    }
  }
}

public class RoomsSelectionScene : Scene
{
  private SelectionList<string> roomsList = null!;
  private readonly RoomsService roomsService = new();
  private const int ITEM_HEIGHT = 30;
  private const int PADDING = 20;
  private const int HEADER_HEIGHT = 40;
  private int scrollOffset = 0;
  Color backgroundColor = new(0, 0, 0, 220);

  public RoomsSelectionScene()
  {
    LoadAvailableRooms();
  }

  private void LoadAvailableRooms()
  {
    var rooms = roomsService.GetAllRooms().ToList();
    roomsList = new SelectionList<string>(rooms);
    if (rooms.Count > 0)
    {
      roomsList.SelectNext();
    }
  }

  public override void OnEnter()
  {
    base.OnEnter();
    LoadAvailableRooms();
  }

  public override void OnExit()
  {
    base.OnExit();
    roomsList = null!;
  }

  public override void Update(float dt)
  {
    var previousSelectedIndex = roomsList.SelectedIndex;
    roomsList.Update(dt);

    if (previousSelectedIndex != roomsList.SelectedIndex)
    {
      UpdateScrollPosition();
    }

    if (IsKeyPressed(KeyboardKey.Enter) && roomsList.SelectedIndex >= 0)
    {
      OnRoomSelected(roomsList.Items[roomsList.SelectedIndex]);
    }

    if (IsKeyPressed(KeyboardKey.Escape))
    {
      ScenesController.PopScene();
    }
  }

  private void UpdateScrollPosition()
  {
    if (roomsList.Items.Count == 0) return;

    int screenHeight = GetScreenHeight();
    int availableHeight = screenHeight - HEADER_HEIGHT - PADDING * 2 - 30; // 30 for instructions
    int visibleItems = availableHeight / ITEM_HEIGHT;

    // Auto-scroll to keep selected item visible
    if (roomsList.SelectedIndex < scrollOffset)
    {
      // Selected item is above visible area, scroll up
      scrollOffset = roomsList.SelectedIndex;
    }
    else if (roomsList.SelectedIndex >= scrollOffset + visibleItems)
    {
      // Selected item is below visible area, scroll down
      scrollOffset = roomsList.SelectedIndex - visibleItems + 1;
    }

    // Ensure scroll offset doesn't go out of bounds
    scrollOffset = Math.Max(0, Math.Min(scrollOffset, Math.Max(0, roomsList.Items.Count - visibleItems)));
  }

  public override void Draw()
  {
    base.Draw();
    DrawRectangle(0, 0, GetScreenWidth(), GetScreenHeight(), backgroundColor);

    int screenWidth = GetScreenWidth();
    int screenHeight = GetScreenHeight();

    // Header
    DrawTextEx(Constants.DefaultFont, "Select Room",
               new Vector2(PADDING, PADDING), 24, 1, Color.White);

    // Calculate visible area for scrolling
    int listStartY = PADDING + HEADER_HEIGHT;
    int availableHeight = screenHeight - listStartY - PADDING - 30; // 30 for instructions
    int visibleItems = availableHeight / ITEM_HEIGHT;
    int listWidth = screenWidth - (PADDING * 2);

    // Draw visible portion of the rooms list using ListSelection component
    roomsList.Draw(PADDING, listStartY, listWidth, ITEM_HEIGHT, 20, scrollOffset, visibleItems);

    // Draw scroll indicator if needed
    if (roomsList.Items.Count > visibleItems)
    {
      DrawScrollIndicator(screenWidth, listStartY, availableHeight);
    }

    // Draw instructions at bottom
    int instructionsY = screenHeight - 30;
    DrawTextEx(
      Constants.DefaultFont,
      "Use [ up / down ] to navigate, Enter to select, Esc to cancel",
      new Vector2(PADDING, instructionsY), 16, 1, Color.LightGray);
  }

  private void DrawScrollIndicator(int screenWidth, int listStartY, int availableHeight)
  {
    if (roomsList.Items.Count <= 1) return;

    int scrollBarWidth = 8;
    int scrollBarX = screenWidth - PADDING - scrollBarWidth;
    int scrollBarHeight = availableHeight;

    // Draw scroll track
    DrawRectangle(scrollBarX, listStartY, scrollBarWidth, scrollBarHeight, Color.DarkGray);

    // Calculate thumb size and position
    float thumbRatio = Math.Min(1.0f, (float)availableHeight / ITEM_HEIGHT / roomsList.Items.Count);
    int thumbHeight = Math.Max(20, (int)(scrollBarHeight * thumbRatio));

    float scrollProgress = roomsList.Items.Count > 1 ? (float)scrollOffset / (roomsList.Items.Count - 1) : 0;
    int thumbY = listStartY + (int)((scrollBarHeight - thumbHeight) * scrollProgress);

    // Draw scroll thumb
    DrawRectangle(scrollBarX, thumbY, scrollBarWidth, thumbHeight, Color.LightGray);
  }

  private static void OnRoomSelected(string roomName)
  {
    // Build the full path to the room file
    string fullRoomPath = FilePathService.GetRoomPath(roomName);

    GameEditorViewModel.LoadTilemap(fullRoomPath);
    ScenesController.PopAll();
    ScenesController.SwitchTo<RoomEditorScene>();
  }


}