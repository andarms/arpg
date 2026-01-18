namespace Arpg.Editor;

public class TilesetPanel(string path)
{
  readonly TilesetViewModel tileset = new(path);
  const int PADDING = 10;

  public Vector2 Position = new(PADDING, GetScreenHeight() - 214 - PADDING);

  const int TILE_SIZE = 16;
  const int spacing = 2;
  const int scale = 2;
  const int scaledTileSize = TILE_SIZE * scale;

  const int TILES_PER_PAGE = 25 * 6;

  private int currentPage = 0;

  public IEnumerable<Rectangle> TilesOnPage => tileset.Tiles.Skip(currentPage * TILES_PER_PAGE).Take(TILES_PER_PAGE);

  public void Update()
  {
    if (IsMouseButtonPressed(MouseButton.Left))
    {
      Vector2 mousePosition = GetMousePosition();
      for (int i = 0; i < TilesOnPage.Count(); i++)
      {
        int x = i % 25;
        int y = i / 25;
        Vector2 position = Position + new Vector2(x * (scaledTileSize + spacing) + PADDING, y * (scaledTileSize + spacing) + PADDING);
        Rectangle destination = new Rectangle(position.X, position.Y, scaledTileSize, scaledTileSize);
        if (CheckCollisionPointRec(mousePosition, destination))
        {
          tileset.SelectedTileIndex = i;
          break;
        }
      }
    }
  }

  public void Draw()
  {
    for (int i = 0; i < TilesOnPage.Count(); i++)
    {
      int x = i % 25;
      int y = i / 25;
      Vector2 position = Position + new Vector2(x * (scaledTileSize + spacing) + PADDING, y * (scaledTileSize + spacing) + PADDING);
      Rectangle destination = new Rectangle(position.X, position.Y, scaledTileSize, scaledTileSize);
      DrawTexturePro(tileset.Texture, TilesOnPage.ElementAt(i), destination, Vector2.Zero, 0.0f, Color.White);
    }

    if (tileset.SelectedTileIndex != -1)
    {
      int x = tileset.SelectedTileIndex % 25;
      int y = tileset.SelectedTileIndex / 25;
      Vector2 position = Position + new Vector2(x * (scaledTileSize + spacing) + PADDING, y * (scaledTileSize + spacing) + PADDING);
      Rectangle destination = new(position.X, position.Y, scaledTileSize, scaledTileSize);
      DrawRectangleLinesEx(destination, 3, Color.Red);
      DrawTexturePro(tileset.Texture, TilesOnPage.ElementAt(tileset.SelectedTileIndex), new Rectangle(Position.X + PADDING, Position.Y - 66, TILE_SIZE * 4, TILE_SIZE * 4), Vector2.Zero, 0.0f, Color.White);
    }
  }
}
