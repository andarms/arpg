
namespace Arpg.Editor;


public class MapPanel
{
  public Vector2 Position = new(Settings.Padding, Settings.Padding);

  const int cols = 25;
  const int rows = 12;

  Rectangle bounds;
  bool showGrid = true;

  public MapPanel()
  {
    bounds = new Rectangle(Position.X, Position.Y, cols * Settings.ScaledTileSize, rows * Settings.ScaledTileSize);
  }


  void DrawGrid()
  {
    if (!showGrid) return;

    for (int x = 0; x <= cols; x++)
    {
      DrawLineV(new Vector2(Position.X + x * Settings.ScaledTileSize, Position.Y), new Vector2(Position.X + x * Settings.ScaledTileSize, Position.Y + rows * Settings.ScaledTileSize), Color.Gray);
    }
    for (int y = 0; y <= rows; y++)
    {
      DrawLineV(new Vector2(Position.X, Position.Y + y * Settings.ScaledTileSize), new Vector2(Position.X + cols * Settings.ScaledTileSize, Position.Y + y * Settings.ScaledTileSize), Color.Gray);
    }
  }


  public void Update()
  {
    if (IsMouseButtonDown(MouseButton.Left))
    {
      var (x, y) = GetTileCoordinatesAtMouse();
      GameEditorViewModel.Tilemap?.SetTile(GameEditorViewModel.SelectedLayer, x, y, GameEditorViewModel.Tileset.SelectedTileIndex);
    }

    if (IsMouseButtonDown(MouseButton.Right))
    {
      var (x, y) = GetTileCoordinatesAtMouse();
      GameEditorViewModel.Tilemap?.EraseTile(GameEditorViewModel.SelectedLayer, x, y);
    }

    if (IsKeyPressed(KeyboardKey.F1))
    {
      showGrid = !showGrid;
    }
  }


  public (int, int) GetTileCoordinatesAtMouse()
  {
    Vector2 mousePos = GetMousePosition();
    if (CheckCollisionPointRec(mousePos, bounds))
    {
      int x = (int)((mousePos.X - Position.X) / Settings.ScaledTileSize);
      int y = (int)((mousePos.Y - Position.Y) / Settings.ScaledTileSize);
      return (x, y);
    }
    // viewmodel will handle this -1 case when out of bounds
    return (-1, -1);
  }


  public void Draw()
  {
    GameEditorViewModel.Tilemap?.Draw();
    DrawGrid();
  }
}