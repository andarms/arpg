namespace Arpg.Editor;

public class TilesetPanel
{

  public Vector2 Position = new(0, GetScreenHeight() - 214 - Settings.Padding);

  const int spacing = 2;
  const int panelTilesWide = 26;

  const int TILES_PER_PAGE = panelTilesWide * 6;

  private int currentPage = 0;

  Rectangle pageUpButton;
  Rectangle pageDownButton;
  Rectangle upArrowSource;
  Rectangle downArrowSource;

  private readonly int gridRows = 6;
  private readonly int gridCols = panelTilesWide;

  public IEnumerable<Rectangle> TilesOnPage => GameEditorViewModel.Tileset.Tiles.Skip(currentPage * TILES_PER_PAGE).Take(TILES_PER_PAGE);

  public TilesetPanel()
  {
    pageUpButton = new Rectangle(Position.X + (panelTilesWide * (Settings.ScaledTileSize + spacing)) + 32, Position.Y + Settings.Padding, 32, 32);
    pageDownButton = new Rectangle(Position.X + (panelTilesWide * (Settings.ScaledTileSize + spacing)) + 32, Position.Y + Settings.Padding + 40, 32, 32);
    upArrowSource = new Rectangle(160, 64, 16, 16);
    downArrowSource = new Rectangle(176, 64, 16, 16);
  }

  public int TotalPages => (int)Math.Ceiling((double)GameEditorViewModel.Tileset.Tiles.Count / TILES_PER_PAGE);

  public bool CanPageUp => currentPage > 0;

  public bool CanPageDown => currentPage < TotalPages - 1;
  public void Update()
  {
    if (IsMouseButtonPressed(MouseButton.Left))
    {
      Vector2 mousePosition = GetMousePosition();

      // Check page navigation buttons
      if (CheckCollisionPointRec(mousePosition, pageUpButton) && CanPageUp)
      {
        currentPage--;
        return;
      }

      if (CheckCollisionPointRec(mousePosition, pageDownButton) && CanPageDown)
      {
        currentPage++;
        return;
      }

      // Check tile selection
      for (int i = 0; i < TilesOnPage.Count(); i++)
      {
        int x = i % panelTilesWide;
        int y = i / panelTilesWide;
        Vector2 position = Position + new Vector2(x * (Settings.ScaledTileSize + spacing) + Settings.Padding, y * (Settings.ScaledTileSize + spacing) + Settings.Padding);
        Rectangle destination = new(position.X, position.Y, Settings.ScaledTileSize, Settings.ScaledTileSize);
        if (CheckCollisionPointRec(mousePosition, destination))
        {
          // Calculate absolute tile index across all pages
          GameEditorViewModel.Tileset.SelectedTileIndex = currentPage * TILES_PER_PAGE + i;
          break;
        }
      }
    }

    // Check keyboard navigation
    if (IsKeyPressed(KeyboardKey.PageUp) && CanPageUp)
    {
      currentPage--;
    }
    else if (IsKeyPressed(KeyboardKey.PageDown) && CanPageDown)
    {
      currentPage++;
    }
  }

  public void Draw()
  {
    // Draw grid
    // Draw horizontal grid lines
    for (int row = 0; row <= gridRows; row++)
    {
      float y = Position.Y + Settings.Padding + (row * (Settings.ScaledTileSize + spacing));
      float startX = Position.X + Settings.Padding;
      float endX = Position.X + Settings.Padding + (gridCols * (Settings.ScaledTileSize + spacing));
      DrawLineEx(new Vector2(startX, y), new Vector2(endX, y), 1, Color.LightGray);
    }

    // Draw vertical grid lines
    for (int col = 0; col <= gridCols; col++)
    {
      float x = Position.X + Settings.Padding + (col * (Settings.ScaledTileSize + spacing));
      float startY = Position.Y + Settings.Padding;
      float endY = Position.Y + Settings.Padding + (gridRows * (Settings.ScaledTileSize + spacing));
      DrawLineEx(new Vector2(x, startY), new Vector2(x, endY), 1, Color.LightGray);
    }

    // Draw tiles
    for (int i = 0; i < TilesOnPage.Count(); i++)
    {
      int x = i % panelTilesWide;
      int y = i / panelTilesWide;
      Vector2 position = Position + new Vector2(x * (Settings.ScaledTileSize + spacing) + Settings.Padding, y * (Settings.ScaledTileSize + spacing) + Settings.Padding);
      Rectangle destination = new Rectangle(position.X, position.Y, Settings.ScaledTileSize, Settings.ScaledTileSize);
      DrawTexturePro(GameEditorViewModel.Tileset.Texture, TilesOnPage.ElementAt(i), destination, Vector2.Zero, 0.0f, Color.White);
    }

    // Draw page navigation buttons
    // Up arrow button
    Color upButtonColor = CanPageUp ? Color.White : Color.Gray;
    DrawTexturePro(Settings.CursorTexture, upArrowSource, pageUpButton, Vector2.Zero, 0.0f, upButtonColor);

    // Down arrow button
    Color downButtonColor = CanPageDown ? Color.White : Color.Gray;
    DrawTexturePro(Settings.CursorTexture, downArrowSource, pageDownButton, Vector2.Zero, 0.0f, downButtonColor);

    // Draw page indicator
    string pageText = $"Page {currentPage + 1} of {TotalPages}";
    Vector2 pageTextPosition = new Vector2(pageUpButton.X, pageDownButton.Y + 40);
    DrawTextEx(Settings.DefaultFont, pageText, pageTextPosition, 32, 0, Color.White);

    // Draw selection highlight and preview
    if (GameEditorViewModel.Tileset.SelectedTileIndex != -1)
    {
      // Calculate relative position of selected tile on current page
      int relativeIndex = GameEditorViewModel.Tileset.SelectedTileIndex - (currentPage * TILES_PER_PAGE);

      // Only draw selection if the selected tile is on the current page
      if (relativeIndex >= 0 && relativeIndex < TilesOnPage.Count())
      {
        int x = relativeIndex % panelTilesWide;
        int y = relativeIndex / panelTilesWide;
        Vector2 position = Position + new Vector2(x * (Settings.ScaledTileSize + spacing) + Settings.Padding, y * (Settings.ScaledTileSize + spacing) + Settings.Padding);
        Rectangle destination = new(position.X, position.Y, Settings.ScaledTileSize, Settings.ScaledTileSize);
        DrawRectangleLinesEx(destination, 3, Color.Red);
      }

      // Always draw the preview using the absolute selected tile index
      Rectangle selectedTileRect = GameEditorViewModel.Tileset.Tiles[GameEditorViewModel.Tileset.SelectedTileIndex];
      DrawTexturePro(GameEditorViewModel.Tileset.Texture, selectedTileRect, new Rectangle(Position.X + Settings.Padding, Position.Y - 66, Settings.TileSize * 4, Settings.TileSize * 4), Vector2.Zero, 0.0f, Color.White);
    }
  }
}
