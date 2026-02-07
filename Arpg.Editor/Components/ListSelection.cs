namespace Arpg.Editor.Components;

public class ListSelection<T>
{
  public List<T> Items { get; private set; }
  public int SelectedIndex { get; private set; } = -1;

  public ListSelection(List<T> items)
  {
    Items = items;
  }

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