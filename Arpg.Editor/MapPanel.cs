namespace Arpg.Editor;

public class MapPanel
{
  public Vector2 Position = new(Constants.Padding, Constants.Padding * 3);

  const int cols = 25;
  const int rows = 20;
  const int CollisionGridSize = 8; // 8px grid for collision rectangles
  const int ScrollSpeed = 32; // pixels to scroll per arrow key press
  const int ScrollbarWidth = 8; // Width/height of scrollbars
  const int ScrollbarPadding = 2; // Padding around scrollbar elements

  Rectangle bounds;

  // Camera offset for scrolling through larger maps
  public Vector2 CameraOffset = Vector2.Zero;

  // Collision rectangle drawing state
  Vector2? collisionStartPos = null;
  bool isDrawingCollision = false;

  public MapPanel()
  {
    bounds = new Rectangle(Position.X, Position.Y, cols * Constants.ScaledTileSize, rows * Constants.ScaledTileSize);
  }

  public void Update()
  {
    if (!GameEditorViewModel.Tilemap?.IsLoaded ?? true)
    {
      return;
    }

    // Handle collision rectangle creation for Collision layer
    if (GameEditorViewModel.SelectedLayer == (int)TileLayer.Collision)
    {
      HandleCollisionRectangleInput();
    }
    else
    {
      // Handle regular tile placement for other layers
      HandleTileInput();
    }

    // Handle camera scrolling
    HandleCameraScrolling();

    // Global keyboard shortcuts
    HandleGlobalShortcuts();
  }

  void HandleCollisionRectangleInput()
  {
    Vector2 mousePos = GetMousePosition();
    if (!CheckCollisionPointRec(mousePos, bounds))
    {
      return;
    }

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
        // Convert from editor coordinates to world coordinates, accounting for camera offset
        Vector2 worldPosition = (topLeft - Position - CameraOffset) / Constants.Scale;
        Vector2 worldSize = size / Constants.Scale;

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
      Vector2 worldPos = (mousePos - Position - CameraOffset) / Constants.Scale;
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

  void HandleCameraScrolling()
  {
    if (!GameEditorViewModel.Tilemap?.IsLoaded ?? true)
    {
      return;
    }

    Vector2 scrollDelta = Vector2.Zero;

    if (IsKeyPressed(KeyboardKey.Up))
    {
      scrollDelta.Y = ScrollSpeed;
    }

    if (IsKeyPressed(KeyboardKey.Down))
    {
      scrollDelta.Y = -ScrollSpeed;
    }

    if (IsKeyPressed(KeyboardKey.Left))
    {
      scrollDelta.X = ScrollSpeed;
    }

    if (IsKeyPressed(KeyboardKey.Right))
    {
      scrollDelta.X = -ScrollSpeed;
    }

    if (scrollDelta != Vector2.Zero)
    {
      CameraOffset += scrollDelta;
      ClampCameraOffset();
    }
  }

  void ClampCameraOffset()
  {
    if (!GameEditorViewModel.Tilemap?.IsLoaded ?? true)
    {
      return;
    }

    // Calculate the max scroll bounds based on map size
    int mapWidthPx = GameEditorViewModel.Tilemap!.Width * Constants.ScaledTileSize;
    int mapHeightPx = GameEditorViewModel.Tilemap!.Height * Constants.ScaledTileSize;
    int viewWidthPx = cols * Constants.ScaledTileSize;
    int viewHeightPx = rows * Constants.ScaledTileSize;

    // Allow camera to scroll if map is larger than viewport
    float maxOffsetX = Math.Max(0, mapWidthPx - viewWidthPx);
    float maxOffsetY = Math.Max(0, mapHeightPx - viewHeightPx);

    CameraOffset.X = Math.Clamp(CameraOffset.X, -maxOffsetX, 0);
    CameraOffset.Y = Math.Clamp(CameraOffset.Y, -maxOffsetY, 0);
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
        GameEditorViewModel.SelectedLayer = (int)TileLayer.Layer1;
      }
    }

    if (IsKeyPressed(KeyboardKey.S))
    {
      GameEditorViewModel.Tilemap?.Save();
    }

    if (IsKeyPressed(KeyboardKey.Q))
    {
      GameEditorViewModel.LoadTilemap("map.data");
      CameraOffset = Vector2.Zero; // Reset camera when loading a map
    }

    if (IsKeyPressed(KeyboardKey.N))
    {
      GameEditorViewModel.CreateTilemap(30, 30, "Textures/tileset.png");
      CameraOffset = Vector2.Zero; // Reset camera when creating a new map
    }
  }

  Vector2 SnapToCollisionGrid(Vector2 position)
  {
    Vector2 relativePos = position - Position - CameraOffset;
    float snappedX = MathF.Floor(relativePos.X / CollisionGridSize) * CollisionGridSize;
    float snappedY = MathF.Floor(relativePos.Y / CollisionGridSize) * CollisionGridSize;
    return Position + CameraOffset + new Vector2(snappedX, snappedY);
  }

  public (int, int) GetTileCoordinatesAtMouse()
  {
    Vector2 mousePos = GetMousePosition();
    if (CheckCollisionPointRec(mousePos, bounds))
    {
      // Account for camera offset when calculating tile coordinates
      int x = (int)((mousePos.X - Position.X - CameraOffset.X) / Constants.ScaledTileSize);
      int y = (int)((mousePos.Y - Position.Y - CameraOffset.Y) / Constants.ScaledTileSize);
      return (x, y);
    }
    // Return -1 when out of bounds
    return (-1, -1);
  }

  public void Draw()
  {
    // Enable clipping to map panel bounds
    BeginScissorMode((int)Position.X, (int)Position.Y, cols * Constants.ScaledTileSize, rows * Constants.ScaledTileSize);

    if (GameEditorViewModel.Tilemap?.IsLoaded ?? false)
    {
      // Calculate scale based on Settings.ScaledTileSize vs actual tile size
      int tileSize = GameEditorViewModel.Tilemap.Data?.Tileset.TileWidth ?? 16;
      int scale = Constants.ScaledTileSize / tileSize;

      // Apply camera offset to the tilemap drawing position
      Vector2 drawPosition = Position + CameraOffset;
      GameEditorViewModel.Tilemap.Draw(drawPosition, scale);
    }

    // Draw collision rectangles when on collision layer or for reference
    if (GameEditorViewModel.SelectedLayer == (int)TileLayer.Collision || GameEditorViewModel.SelectedLayer == -1)
    {
      DrawCollisionRectangles();
    }

    // Draw collision grid when collision layer is selected
    if (GameEditorViewModel.SelectedLayer == (int)TileLayer.Collision)
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

    // End clipping
    EndScissorMode();

    // Draw scrollbars outside the clipped area
    DrawScrollbars();
  }

  void DrawCollisionRectangles()
  {
    if (GameEditorViewModel.Tilemap?.IsLoaded ?? false)
    {
      foreach (var rect in GameEditorViewModel.Tilemap.CollisionRectangles)
      {
        Color color = GameEditorViewModel.SelectedLayer == (int)TileLayer.Collision ? Color.Red : new Color(255, 0, 0, 128);

        // Convert from world coordinates to editor coordinates, accounting for camera offset
        Vector2 editorPos = Position + CameraOffset + (rect.Position * Constants.Scale);
        Vector2 editorSize = rect.Size * Constants.Scale;

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
    if (!GameEditorViewModel.ShowGrid)
    {
      return;
    }

    if (!GameEditorViewModel.Tilemap?.IsLoaded ?? true)
    {
      return;
    }

    // Calculate grid for entire map, not just viewport
    int mapWidthPx = GameEditorViewModel.Tilemap!.Width * Constants.ScaledTileSize;
    int mapHeightPx = GameEditorViewModel.Tilemap!.Height * Constants.ScaledTileSize;
    int gridCols = mapWidthPx / CollisionGridSize;
    int gridRows = mapHeightPx / CollisionGridSize;

    Vector2 gridPosition = Position + CameraOffset;

    // Draw vertical lines for entire map width
    for (int x = 0; x <= gridCols; x++)
    {
      DrawLineV(
        new Vector2(gridPosition.X + x * CollisionGridSize, gridPosition.Y),
        new Vector2(gridPosition.X + x * CollisionGridSize, gridPosition.Y + mapHeightPx),
        new Color(255, 255, 0, 30)
      );
    }

    // Draw horizontal lines for entire map height
    for (int y = 0; y <= gridRows; y++)
    {
      DrawLineV(
        new Vector2(gridPosition.X, gridPosition.Y + y * CollisionGridSize),
        new Vector2(gridPosition.X + mapWidthPx, gridPosition.Y + y * CollisionGridSize),
        new Color(255, 255, 0, 30)
      );
    }
  }

  void DrawCurrentCollisionRectangle()
  {
    if (!collisionStartPos.HasValue)
    {
      return;
    }

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
    if (!GameEditorViewModel.ShowGrid)
    {
      return;
    }

    if (!GameEditorViewModel.Tilemap?.IsLoaded ?? true)
    {
      return;
    }

    Vector2 gridPosition = Position + CameraOffset;

    // Draw grid for entire map, not just viewport
    int mapCols = GameEditorViewModel.Tilemap?.Width ?? cols;
    int mapRows = GameEditorViewModel.Tilemap?.Height ?? rows;

    // Draw vertical lines for all tiles
    for (int x = 0; x <= mapCols; x++)
    {
      DrawLineV(
        new Vector2(gridPosition.X + x * Constants.ScaledTileSize, gridPosition.Y),
        new Vector2(gridPosition.X + x * Constants.ScaledTileSize, gridPosition.Y + mapRows * Constants.ScaledTileSize),
        Color.LightGray
      );
    }

    // Draw horizontal lines for all tiles
    for (int y = 0; y <= mapRows; y++)
    {
      DrawLineV(
        new Vector2(gridPosition.X, gridPosition.Y + y * Constants.ScaledTileSize),
        new Vector2(gridPosition.X + mapCols * Constants.ScaledTileSize, gridPosition.Y + y * Constants.ScaledTileSize),
        Color.LightGray
      );
    }
  }

  void DrawScrollbars()
  {
    if (!GameEditorViewModel.Tilemap?.IsLoaded ?? true)
    {
      return;
    }

    int mapWidthPx = GameEditorViewModel.Tilemap!.Width * Constants.ScaledTileSize;
    int mapHeightPx = GameEditorViewModel.Tilemap!.Height * Constants.ScaledTileSize;
    int viewportWidth = cols * Constants.ScaledTileSize;
    int viewportHeight = rows * Constants.ScaledTileSize;

    // Only draw scrollbars if map is larger than viewport
    bool needsHorizontalScrollbar = mapWidthPx > viewportWidth;
    bool needsVerticalScrollbar = mapHeightPx > viewportHeight;

    if (needsHorizontalScrollbar)
    {
      DrawHorizontalScrollbar(mapWidthPx, viewportWidth);
    }

    if (needsVerticalScrollbar)
    {
      DrawVerticalScrollbar(mapHeightPx, viewportHeight);
    }
  }

  void DrawHorizontalScrollbar(int mapWidth, int viewportWidth)
  {
    // Calculate scrollbar dimensions and position
    float scrollbarY = Position.Y + rows * Constants.ScaledTileSize + ScrollbarPadding;
    float scrollbarTrackWidth = cols * Constants.ScaledTileSize;

    // Draw scrollbar track (background)
    DrawRectangle(
      (int)Position.X,
      (int)scrollbarY,
      (int)scrollbarTrackWidth,
      ScrollbarWidth,
      Color.DarkGray
    );

    // Calculate thumb dimensions and position
    float thumbWidthRatio = (float)viewportWidth / mapWidth;
    float thumbWidth = scrollbarTrackWidth * thumbWidthRatio;
    float maxScrollOffset = mapWidth - viewportWidth;
    float scrollProgress = maxScrollOffset > 0 ? Math.Abs(CameraOffset.X) / maxScrollOffset : 0;
    float thumbX = Position.X + (scrollbarTrackWidth - thumbWidth) * scrollProgress;

    // Draw scrollbar thumb (handle)
    DrawRectangle(
      (int)thumbX,
      (int)(scrollbarY + ScrollbarPadding),
      (int)thumbWidth,
      ScrollbarWidth - (ScrollbarPadding * 2),
      Color.LightGray
    );

    // Draw thumb border
    DrawRectangleLines(
      (int)thumbX,
      (int)(scrollbarY + ScrollbarPadding),
      (int)thumbWidth,
      ScrollbarWidth - (ScrollbarPadding * 2),
      Color.Gray
    );
  }

  void DrawVerticalScrollbar(int mapHeight, int viewportHeight)
  {
    // Calculate scrollbar dimensions and position
    float scrollbarX = Position.X + cols * Constants.ScaledTileSize + ScrollbarPadding;
    float scrollbarTrackHeight = rows * Constants.ScaledTileSize;

    // Draw scrollbar track (background)
    DrawRectangle(
      (int)scrollbarX,
      (int)Position.Y,
      ScrollbarWidth,
      (int)scrollbarTrackHeight,
      Color.DarkGray
    );

    // Calculate thumb dimensions and position
    float thumbHeightRatio = (float)viewportHeight / mapHeight;
    float thumbHeight = scrollbarTrackHeight * thumbHeightRatio;
    float maxScrollOffset = mapHeight - viewportHeight;
    float scrollProgress = maxScrollOffset > 0 ? Math.Abs(CameraOffset.Y) / maxScrollOffset : 0;
    float thumbY = Position.Y + (scrollbarTrackHeight - thumbHeight) * scrollProgress;

    // Draw scrollbar thumb (handle)
    DrawRectangle(
      (int)(scrollbarX + ScrollbarPadding),
      (int)thumbY,
      ScrollbarWidth - (ScrollbarPadding * 2),
      (int)thumbHeight,
      Color.LightGray
    );

    // Draw thumb border
    DrawRectangleLines(
      (int)(scrollbarX + ScrollbarPadding),
      (int)thumbY,
      ScrollbarWidth - (ScrollbarPadding * 2),
      (int)thumbHeight,
      Color.Gray
    );
  }
}