namespace Arpg.Editor.Components;

public class SelectionList<T>(List<T> items)
{
  public List<T> Items { get; private set; } = items;
  public int SelectedIndex { get; private set; } = -1;

  public void AddItems(List<T> items)
  {
    Items.AddRange(items);
    if (SelectedIndex == -1 && Items.Count > 0)
    {
      SelectedIndex = 0;
    }
  }

  public void Update(float dt)
  {
    if (IsKeyPressed(KeyboardKey.Down))
    {
      SelectNext();
    }
    else if (IsKeyPressed(KeyboardKey.Up))
    {
      SelectPrevious();
    }

    if (IsKeyPressed(KeyboardKey.Enter))
    {
      var selectedItem = GetSelectedItem();
      if (selectedItem != null)
      {
        // Handle selection of the item
        Console.WriteLine($"Selected: {selectedItem}");
      }
    }
  }

  public void Draw(int startX, int startY, int width, int itemHeight, int fontSize = 20, int scrollOffset = 0, int visibleItems = -1)
  {
    // Calculate which items to draw based on scrolling
    int startIndex = scrollOffset;
    int endIndex = visibleItems > 0 ? Math.Min(startIndex + visibleItems, Items.Count) : Items.Count;

    for (int i = startIndex; i < endIndex; i++)
    {
      int displayIndex = i - startIndex;
      int itemY = startY + (displayIndex * itemHeight);
      var itemRect = new Rectangle(startX, itemY, width, itemHeight - 2);

      // Draw selection highlight
      if (i == SelectedIndex)
      {
        DrawRectangle((int)itemRect.X, (int)itemRect.Y, (int)itemRect.Width, (int)itemRect.Height, Color.Blue);
        DrawRectangleLines((int)itemRect.X, (int)itemRect.Y, (int)itemRect.Width, (int)itemRect.Height, Color.SkyBlue);
      }

      // Draw item text
      var color = i == SelectedIndex ? Color.White : Color.LightGray;
      string itemText = Items[i]?.ToString() ?? "";
      DrawTextEx(Constants.DefaultFont, itemText,
                 new Vector2((int)itemRect.X + 15, (int)itemRect.Y + (itemHeight - fontSize) / 2), fontSize, 1, color);
    }
  }

  public void SelectNext()
  {
    if (Items.Count == 0) return;
    SelectedIndex = (SelectedIndex + 1) % Items.Count;
  }

  public void SelectPrevious()
  {
    if (Items.Count == 0) return;
    SelectedIndex = (SelectedIndex - 1 + Items.Count) % Items.Count;
  }

  public T? GetSelectedItem()
  {
    if (SelectedIndex >= 0 && SelectedIndex < Items.Count)
    {
      return Items[SelectedIndex];
    }
    return default;
  }
}