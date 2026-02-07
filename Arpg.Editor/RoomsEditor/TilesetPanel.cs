namespace Arpg.Editor.RoomsEditor;

public class TilesetPanel
{

  public Vector2 Position = new(GetScreenWidth() - 416 - Constants.Padding, 146 + Constants.Padding);

  const int spacing = 2;
  const int panelTilesWide = 12;

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
    pageUpButton = new Rectangle(Position.X + 128, Position.Y, 32, 32);
    pageDownButton = new Rectangle(Position.X + 128, Position.Y + 32, 32, 32);
    upArrowSource = new Rectangle(160, 64, 16, 16);
    downArrowSource = new Rectangle(176, 64, 16, 16);
  }

  public static int TotalPages => (int)Math.Ceiling((double)GameEditorViewModel.Tileset.Tiles.Count / TILES_PER_PAGE);

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
        float gridOffsetY = Constants.TileSize * 4 + Constants.Padding;
        Vector2 position = Position + new Vector2(x * (Constants.ScaledTileSize + spacing), gridOffsetY + y * (Constants.ScaledTileSize + spacing));
        Rectangle destination = new(position.X, position.Y, Constants.ScaledTileSize, Constants.ScaledTileSize);
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
    // Draw the tileset grid
    DrawTilesetGrid();

    // Draw page navigation buttons
    // Up arrow button
    Color upButtonColor = CanPageUp ? Color.White : Color.Gray;
    DrawTexturePro(Constants.CursorTexture, upArrowSource, pageUpButton, Vector2.Zero, 0.0f, upButtonColor);

    // Down arrow button
    Color downButtonColor = CanPageDown ? Color.White : Color.Gray;
    DrawTexturePro(Constants.CursorTexture, downArrowSource, pageDownButton, Vector2.Zero, 0.0f, downButtonColor);

    // Draw page indicator
    string pageText = $"Page {currentPage + 1} of {TotalPages}";
    Vector2 pageTextPosition = new Vector2(pageUpButton.X + 48, pageUpButton.Y);
    DrawTextEx(Constants.DefaultFont, pageText, pageTextPosition, 32, 0, Color.White);

    // Draw the tile preview
    DrawTilePreview();
  }



  void DrawTilePreview()
  {
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
        float gridOffsetY = Constants.TileSize * 4 + Constants.Padding;
        Vector2 position = Position + new Vector2(x * (Constants.ScaledTileSize + spacing), gridOffsetY + y * (Constants.ScaledTileSize + spacing));
        Rectangle destination = new(position.X, position.Y, Constants.ScaledTileSize, Constants.ScaledTileSize);
        DrawRectangleLinesEx(destination, 3, Color.Red);
      }

      // Draw the preview aligned with the grid - center it horizontally
      Rectangle selectedTileRect = GameEditorViewModel.Tileset.Tiles[GameEditorViewModel.Tileset.SelectedTileIndex];
      float previewSize = Constants.TileSize * 4;
      DrawTexturePro(GameEditorViewModel.Tileset.Texture, selectedTileRect, new Rectangle(Position.X, Position.Y, previewSize, previewSize), Vector2.Zero, 0.0f, Color.White);
    }
  }

  void DrawTilesetGrid()
  {
    // Offset the grid down to make space for the tile preview
    float gridOffsetY = Constants.TileSize * 4 + Constants.Padding;

    // Draw horizontal grid lines
    for (int row = 0; row <= gridRows; row++)
    {
      float y = Position.Y + gridOffsetY + (row * (Constants.ScaledTileSize + spacing));
      float startX = Position.X;
      float endX = Position.X + Constants.Padding + (gridCols * (Constants.ScaledTileSize + spacing));
      DrawLineEx(new Vector2(startX, y), new Vector2(endX, y), 1, Color.LightGray);
    }

    // Draw vertical grid lines
    for (int col = 0; col <= gridCols; col++)
    {
      float x = Position.X + (col * (Constants.ScaledTileSize + spacing));
      float startY = Position.Y + gridOffsetY;
      float endY = Position.Y + gridOffsetY + (gridRows * (Constants.ScaledTileSize + spacing));
      DrawLineEx(new Vector2(x, startY), new Vector2(x, endY), 1, Color.LightGray);
    }

    // Draw tiles
    for (int i = 0; i < TilesOnPage.Count(); i++)
    {
      int x = i % panelTilesWide;
      int y = i / panelTilesWide;
      Vector2 position = Position + new Vector2(x * (Constants.ScaledTileSize + spacing), gridOffsetY + y * (Constants.ScaledTileSize + spacing));
      Rectangle destination = new Rectangle(position.X, position.Y, Constants.ScaledTileSize, Constants.ScaledTileSize);
      DrawTexturePro(GameEditorViewModel.Tileset.Texture, TilesOnPage.ElementAt(i), destination, Vector2.Zero, 0.0f, Color.White);
    }
  }

}
