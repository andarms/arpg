
using Arpg.Engine.Tilemaps;

namespace Arpg.Editor;

public class TilemapViewModel
{
  private TilemapData? tilemapData;

  public TilemapData? Data => tilemapData;
  public int Width => tilemapData?.Width ?? 0;
  public int Height => tilemapData?.Height ?? 0;
  public bool IsLoaded => tilemapData != null;

  // Collision rectangle management
  public List<TilemapData.CollisionRectangle> CollisionRectangles => tilemapData?.CollisionRectangles ?? [];

  public void AddCollisionRectangle(Vector2 position, Vector2 size, bool solid = true)
  {
    if (tilemapData == null) return;

    tilemapData.CollisionRectangles.Add(new TilemapData.CollisionRectangle(position, size, solid));
  }

  public void RemoveCollisionRectangle(int index)
  {
    if (tilemapData == null || index < 0 || index >= tilemapData.CollisionRectangles.Count) return;

    tilemapData.CollisionRectangles.RemoveAt(index);
  }

  public int FindCollisionRectangleAt(Vector2 point)
  {
    if (tilemapData == null) return -1;

    for (int i = tilemapData.CollisionRectangles.Count - 1; i >= 0; i--)
    {
      var rect = tilemapData.CollisionRectangles[i];
      var bounds = new Rectangle((int)rect.Position.X, (int)rect.Position.Y, (int)rect.Size.X, (int)rect.Size.Y);
      if (CheckCollisionPointRec(point, bounds))
      {
        return i;
      }
    }

    return -1;
  }

  public void NewMap(int width, int height, string? tilesetPath = null)
  {
    tilemapData = TilemapService.CreateNew(width, height, tilesetPath);
  }

  public void Load(string filePath)
  {
    try
    {
      tilemapData = TilemapService.LoadFromFile(filePath);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Failed to load tilemap: {ex.Message}");
      // Could add error handling/notification to UI here
    }
  }

  public void Save(string filePath = "map.data")
  {
    if (tilemapData == null) return;

    try
    {
      TilemapService.SaveToFile(tilemapData, filePath);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Failed to save tilemap: {ex.Message}");
      // Could add error handling/notification to UI here
    }
  }

  public void SetTile(int layerIndex, int x, int y, int tileIndex)
  {
    tilemapData?.SetTile(layerIndex, x, y, tileIndex);
  }

  public int GetTile(int layerIndex, int x, int y)
  {
    return tilemapData?.GetTile(layerIndex, x, y) ?? -1;
  }

  public void EraseTile(int layerIndex, int x, int y)
  {
    tilemapData?.EraseTile(layerIndex, x, y);
  }

  public void FillLayer(int layerIndex, int tileIndex)
  {
    tilemapData?.FillLayer(layerIndex, tileIndex);
  }

  public void ClearLayer(int layerIndex)
  {
    tilemapData?.ClearLayer(layerIndex);
  }

  public void Draw(Vector2 offset = default, int scale = 1)
  {
    tilemapData?.Draw(offset, scale);
  }
}
