namespace Arpg.Editor;

public class MapPanel
{
  public Vector2 Position = new(Settings.Padding, Settings.Padding);

  const int cols = 25;
  const int rows = 12;
  const int CollisionGridSize = 8; // 8px grid for collision rectangles

  Rectangle bounds;

  // Collision rectangle drawing state
  Vector2? collisionStartPos = null;
  bool isDrawingCollision = false;

  public MapPanel()
  {
    bounds = new Rectangle(Position.X, Position.Y, cols * Settings.ScaledTileSize, rows * Settings.ScaledTileSize);
  }

  public void Update()
  {
    if (!GameEditorViewModel.Tilemap?.IsLoaded ?? true) return;

    // Handle collision rectangle creation for Layer 3
    if (GameEditorViewModel.SelectedLayer == 3)
    {
      HandleCollisionRectangleInput();
    }
    else
    {
      // Handle regular tile placement for other layers
      HandleTileInput();
    }

    // Global keyboard shortcuts
    HandleGlobalShortcuts();
  }

  void HandleCollisionRectangleInput()
  {
    Vector2 mousePos = GetMousePosition();
    if (!CheckCollisionPointRec(mousePos, bounds)) return;

    Vector2 snappedPos = SnapToCollisionGrid(mousePos);

    if (IsMouseButtonPressed(MouseButton.Left))
    {
      collisionStartPos = snappedPos;
      isDrawingCollision = true;
    }

    if (IsMouseButtonReleased(MouseButton.Left) && isDrawingCollision && collisionStartPos.HasValue)
    {
      Vector2 startPos = collisionStartPos.Value;
      Vector2 endPos = snappedPos;

      // Calculate rectangle bounds
      Vector2 topLeft = new Vector2(
        Math.Min(startPos.X, endPos.X),
        Math.Min(startPos.Y, endPos.Y)
      );
      Vector2 bottomRight = new Vector2(
        Math.Max(startPos.X, endPos.X),
        Math.Max(startPos.Y, endPos.Y)
      );
      Vector2 size = bottomRight - topLeft + new Vector2(CollisionGridSize, CollisionGridSize);

      // Add collision rectangle if it has valid size
      if (size.X >= CollisionGridSize && size.Y >= CollisionGridSize)
      {
        // Convert from editor coordinates to world coordinates
        Vector2 worldPosition = (topLeft - Position) / Settings.Scale;
        Vector2 worldSize = size / Settings.Scale;

        GameEditorViewModel.Tilemap?.AddCollisionRectangle(
          worldPosition,
          worldSize,
          true // Default to solid
        );
      }

      collisionStartPos = null;
      isDrawingCollision = false;
    }

    if (IsMouseButtonPressed(MouseButton.Right))
    {
      // Delete collision rectangle at mouse position
      Vector2 worldPos = (mousePos - Position) / Settings.Scale;
      int rectIndex = GameEditorViewModel.Tilemap?.FindCollisionRectangleAt(worldPos) ?? -1;
      if (rectIndex >= 0)
      {
        GameEditorViewModel.Tilemap?.RemoveCollisionRectangle(rectIndex);
      }
    }
  }

  void HandleTileInput()
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
  }

  void HandleGlobalShortcuts()
  {
    if (IsKeyPressed(KeyboardKey.F1))
    {
      if (GameEditorViewModel.ShowGrid)
      {
        GameEditorViewModel.SelectedLayer = -1;
        GameEditorViewModel.ShowGrid = false;
      }
      else
      {
        GameEditorViewModel.ShowGrid = true;
        GameEditorViewModel.SelectedLayer = 0;
      }
    }

    if (IsKeyPressed(KeyboardKey.S))
    {
      GameEditorViewModel.Tilemap?.Save();
    }

    if (IsKeyPressed(KeyboardKey.Q))
    {
      GameEditorViewModel.LoadTilemap("map.data");
    }
  }

  Vector2 SnapToCollisionGrid(Vector2 position)
  {
    Vector2 relativePos = position - Position;
    float snappedX = MathF.Floor(relativePos.X / CollisionGridSize) * CollisionGridSize;
    float snappedY = MathF.Floor(relativePos.Y / CollisionGridSize) * CollisionGridSize;
    return Position + new Vector2(snappedX, snappedY);
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
    // Return -1 when out of bounds
    return (-1, -1);
  }

  public void Draw()
  {
    if (GameEditorViewModel.Tilemap?.IsLoaded ?? false)
    {
      // Calculate scale based on Settings.ScaledTileSize vs actual tile size
      int tileSize = GameEditorViewModel.Tilemap.Data?.Tileset.TileWidth ?? 16;
      int scale = Settings.ScaledTileSize / tileSize;

      GameEditorViewModel.Tilemap.Draw(Position, scale);
    }

    // Draw collision rectangles when on collision layer or for reference
    if (GameEditorViewModel.SelectedLayer == 3 || GameEditorViewModel.SelectedLayer == -1)
    {
      DrawCollisionRectangles();
    }

    // Draw collision grid when collision layer is selected
    if (GameEditorViewModel.SelectedLayer == 3)
    {
      DrawCollisionGrid();
    }
    else
    {
      DrawGrid();
    }

    // Draw current collision rectangle being drawn
    if (isDrawingCollision && collisionStartPos.HasValue)
    {
      DrawCurrentCollisionRectangle();
    }
  }

  void DrawCollisionRectangles()
  {
    if (GameEditorViewModel.Tilemap?.IsLoaded ?? false)
    {
      foreach (var rect in GameEditorViewModel.Tilemap.CollisionRectangles)
      {
        Color color = GameEditorViewModel.SelectedLayer == 3 ? Color.Red : new Color(255, 0, 0, 128);

        // Convert from world coordinates to editor coordinates
        Vector2 editorPos = Position + (rect.Position * Settings.Scale);
        Vector2 editorSize = rect.Size * Settings.Scale;

        DrawRectangleLines(
          (int)editorPos.X,
          (int)editorPos.Y,
          (int)editorSize.X,
          (int)editorSize.Y,
          color
        );

        // Fill with semi-transparent color
        Color fillColor = new Color(255, 0, 0, 64);
        DrawRectangle(
          (int)editorPos.X,
          (int)editorPos.Y,
          (int)editorSize.X,
          (int)editorSize.Y,
          fillColor
        );
      }
    }
  }

  void DrawCollisionGrid()
  {
    if (!GameEditorViewModel.ShowGrid) return;

    int gridCols = (cols * Settings.ScaledTileSize) / CollisionGridSize;
    int gridRows = (rows * Settings.ScaledTileSize) / CollisionGridSize;

    // Draw vertical lines
    for (int x = 0; x <= gridCols; x++)
    {
      DrawLineV(
        new Vector2(Position.X + x * CollisionGridSize, Position.Y),
        new Vector2(Position.X + x * CollisionGridSize, Position.Y + gridRows * CollisionGridSize),
        new Color(255, 255, 0, 30)
      );
    }

    // Draw horizontal lines
    for (int y = 0; y <= gridRows; y++)
    {
      DrawLineV(
        new Vector2(Position.X, Position.Y + y * CollisionGridSize),
        new Vector2(Position.X + gridCols * CollisionGridSize, Position.Y + y * CollisionGridSize),
        new Color(255, 255, 0, 30)
      );
    }
  }

  void DrawCurrentCollisionRectangle()
  {
    if (!collisionStartPos.HasValue) return;

    Vector2 mousePos = GetMousePosition();
    Vector2 snappedMousePos = SnapToCollisionGrid(mousePos);
    Vector2 startPos = collisionStartPos.Value;

    Vector2 topLeft = new Vector2(
      Math.Min(startPos.X, snappedMousePos.X),
      Math.Min(startPos.Y, snappedMousePos.Y)
    );
    Vector2 bottomRight = new Vector2(
      Math.Max(startPos.X, snappedMousePos.X),
      Math.Max(startPos.Y, snappedMousePos.Y)
    );
    Vector2 size = bottomRight - topLeft + new Vector2(CollisionGridSize, CollisionGridSize);

    // Draw preview rectangle
    DrawRectangleLines(
      (int)topLeft.X,
      (int)topLeft.Y,
      (int)size.X,
      (int)size.Y,
      Color.White
    );
  }

  void DrawGrid()
  {
    if (!GameEditorViewModel.ShowGrid) return;

    for (int x = 0; x <= cols; x++)
    {
      DrawLineV(
        new Vector2(Position.X + x * Settings.ScaledTileSize, Position.Y),
        new Vector2(Position.X + x * Settings.ScaledTileSize, Position.Y + rows * Settings.ScaledTileSize),
        Color.Gray
      );
    }

    for (int y = 0; y <= rows; y++)
    {
      DrawLineV(
        new Vector2(Position.X, Position.Y + y * Settings.ScaledTileSize),
        new Vector2(Position.X + cols * Settings.ScaledTileSize, Position.Y + y * Settings.ScaledTileSize),
        Color.Gray
      );
    }
  }
}