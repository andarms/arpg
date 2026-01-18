
namespace Arpg.Editor;

public class MapPanel
{
  public Vector2 Position = new(Settings.Padding, Settings.Padding);

  int cols = 25;
  int rows = 12;


  void DrawGrid()
  {
    for (int x = 0; x <= cols; x++)
    {
      DrawLineV(new Vector2(Position.X + x * Settings.ScaledTileSize, Position.Y), new Vector2(Position.X + x * Settings.ScaledTileSize, Position.Y + rows * Settings.ScaledTileSize), Color.Gray);
    }
    for (int y = 0; y <= rows; y++)
    {
      DrawLineV(new Vector2(Position.X, Position.Y + y * Settings.ScaledTileSize), new Vector2(Position.X + cols * Settings.ScaledTileSize, Position.Y + y * Settings.ScaledTileSize), Color.Gray);
    }
  }


  public void Draw()
  {
    DrawGrid();
  }
}