using Arpg.Engine;

namespace Arpg.Game.Core;

public class Viewport
{
  private Camera2D camera = new()
  {
    Target = new Vector2(0, 0),
    Offset = new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2),
    Rotation = 0.0f,
    Zoom = Settings.Zoom.Default
  };

  public Camera2D Camera => camera;

  public static Vector2 GetScreenCenter() => new(GetScreenWidth() / 2, GetScreenHeight() / 2);


  private Vector2 cachedTopLeft;
  private Vector2 cachedBottomRight;


  public void Initialize()
  {
    camera = new Camera2D
    {
      Target = camera.Target,
      Offset = GetScreenCenter(),
      Rotation = camera.Rotation,
      Zoom = camera.Zoom
    };
  }


  public void Update()
  {
    Vector2 screenCenter = GetScreenCenter();
    cachedTopLeft = camera.Target - screenCenter / camera.Zoom - new Vector2(32);
    cachedBottomRight = camera.Target + screenCenter / camera.Zoom + new Vector2(32);

  }

  public (Vector2 topLeft, Vector2 bottomRight) Area => (cachedTopLeft, cachedBottomRight);

  public void UpdateTarget(Vector2 target)
  {
    Vector2 screenCenter = GetScreenCenter();
    Vector2 cameraLimit = new(
      Game.Limits.X - screenCenter.X + camera.Offset.X / camera.Zoom,
      Game.Limits.Y - screenCenter.Y + camera.Offset.Y / camera.Zoom
    );
    camera.Target = Vector2.Clamp(target, Vector2.Zero + camera.Offset / camera.Zoom, cameraLimit);
  }

  /// <summary>
  /// Checks if a point is within the camera viewport with optional margin
  /// </summary>
  public bool IsPointInView(Vector2 point, float margin = 0f)
  {
    return point.X >= cachedTopLeft.X - margin &&
           point.X <= cachedBottomRight.X + margin &&
           point.Y >= cachedTopLeft.Y - margin &&
           point.Y <= cachedBottomRight.Y + margin;
  }

  /// <summary>
  /// Checks if a rectangle is within the camera viewport
  /// </summary>
  public bool IsRectInView(Vector2 position, Vector2 size)
  {
    return !(position.X + size.X < cachedTopLeft.X ||
             position.X > cachedBottomRight.X ||
             position.Y + size.Y < cachedTopLeft.Y ||
             position.Y > cachedBottomRight.Y);
  }


  public Vector2 GetCameraViewSize()
  {
    Vector2 screenSize = new(GetScreenWidth(), GetScreenHeight());
    return screenSize / camera.Zoom;
  }


  public void SetLimits(int mapWidth, int mapHeight)
  {

    Vector2 cameraViewSize = GetCameraViewSize();
    float mapWidthPx = mapWidth * Settings.Tiles.Size + cameraViewSize.X / 2;
    float mapHeightPx = mapHeight * Settings.Tiles.Size + cameraViewSize.Y / 2;

    float limitX = Math.Max(mapWidthPx, cameraViewSize.X);
    float limitY = Math.Max(mapHeightPx, cameraViewSize.Y);

    Game.Limits = new Vector2(limitX, limitY);
  }

}