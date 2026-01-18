namespace Arpg.Editor;

public class TilesetPanel
{

  public Vector2 Position = new(0, GetScreenHeight() - 214 - Settings.Padding);

  const int spacing = 2;

  const int TILES_PER_PAGE = 25 * 6;

  private int currentPage = 0;

  public IEnumerable<Rectangle> TilesOnPage => GameEditorViewModel.Tileset.Tiles.Skip(currentPage * TILES_PER_PAGE).Take(TILES_PER_PAGE);
  public void Update()
  {
    if (IsMouseButtonPressed(MouseButton.Left))
    {
      Vector2 mousePosition = GetMousePosition();
      for (int i = 0; i < TilesOnPage.Count(); i++)
      {
        int x = i % 25;
        int y = i / 25;
        Vector2 position = Position + new Vector2(x * (Settings.ScaledTileSize + spacing) + Settings.Padding, y * (Settings.ScaledTileSize + spacing) + Settings.Padding);
        Rectangle destination = new(position.X, position.Y, Settings.ScaledTileSize, Settings.ScaledTileSize);
        if (CheckCollisionPointRec(mousePosition, destination))
        {
          GameEditorViewModel.Tileset.SelectedTileIndex = i;
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
      Vector2 position = Position + new Vector2(x * (Settings.ScaledTileSize + spacing) + Settings.Padding, y * (Settings.ScaledTileSize + spacing) + Settings.Padding);
      Rectangle destination = new Rectangle(position.X, position.Y, Settings.ScaledTileSize, Settings.ScaledTileSize);
      DrawTexturePro(GameEditorViewModel.Tileset.Texture, TilesOnPage.ElementAt(i), destination, Vector2.Zero, 0.0f, Color.White);
    }

    if (GameEditorViewModel.Tileset.SelectedTileIndex != -1)
    {
      int x = GameEditorViewModel.Tileset.SelectedTileIndex % 25;
      int y = GameEditorViewModel.Tileset.SelectedTileIndex / 25;
      Vector2 position = Position + new Vector2(x * (Settings.ScaledTileSize + spacing) + Settings.Padding, y * (Settings.ScaledTileSize + spacing) + Settings.Padding);
      Rectangle destination = new(position.X, position.Y, Settings.ScaledTileSize, Settings.ScaledTileSize);
      DrawRectangleLinesEx(destination, 3, Color.Red);
      DrawTexturePro(GameEditorViewModel.Tileset.Texture, TilesOnPage.ElementAt(GameEditorViewModel.Tileset.SelectedTileIndex), new Rectangle(Position.X + Settings.Padding, Position.Y - 66, Settings.TileSize * 4, Settings.TileSize * 4), Vector2.Zero, 0.0f, Color.White);
    }
  }
}
