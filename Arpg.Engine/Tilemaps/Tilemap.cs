using System.Text;
using Arpg.Engine.Gom;

namespace Arpg.Engine.Tilemaps;

public class Tilemap
{
  private const int LayerCount = 3;
  private const int EmptyTile = -1;

  public int Width { get; private set; }
  public int Height { get; private set; }
  public int TotalTiles => Width * Height;
  public TilemapLayer[] Layers { get; private set; } = new TilemapLayer[LayerCount];
  public Tileset Tileset { get; private set; }
  public string TilesetPath { get; private set; } = string.Empty;
  public List<CollisionRectangle> CollisionRectangles { get; private set; } = [];

  // Individual layer properties for direct access
  public TilemapLayer Layer1 => Layers[0];
  public TilemapLayer Layer2 => Layers[1];
  public TilemapLayer Layer3 => Layers[2];

  public class CollisionRectangle
  {
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
    public bool Solid { get; set; } = true;

    public CollisionRectangle(Vector2 position, Vector2 size, bool solid = true)
    {
      Position = position;
      Size = size;
      Solid = solid;
    }
  }

  public Tilemap(int width, int height, Tileset tileset, string tilesetPath = "")
  {
    Width = width;
    Height = height;
    Tileset = tileset;
    TilesetPath = tilesetPath;
    InitializeLayers();
  }

  private void InitializeLayers()
  {
    for (int i = 0; i < LayerCount; i++)
    {
      Layers[i] = new TilemapLayer(Width, Height, $"Layer{i}", Tileset);
    }
  }


  public void Save(string filePath, string assetsPath)
  {
    string path = Path.Combine(assetsPath, filePath);
    StringBuilder sb = new();
    sb.AppendLine("[Tilemap]");
    sb.AppendLine($"Tileset={TilesetPath}");
    sb.AppendLine($"Width={Width},Height={Height}");

    for (int layer = 0; layer < LayerCount; layer++)
    {
      sb.AppendLine($"Layer={layer}");
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          int index = GetTileIndex(x, y);
          int tileIndex = Layers[layer][index];
          sb.Append(tileIndex);
          if (x < Width - 1)
          {
            sb.Append(' ');
          }
        }
        sb.AppendLine();
      }
    }

    // Save collision rectangles
    if (CollisionRectangles.Count > 0)
    {
      sb.AppendLine("[Collisions]");
      foreach (var rect in CollisionRectangles)
      {
        sb.AppendLine($"Rect={rect.Position.X},{rect.Position.Y},{rect.Size.X},{rect.Size.Y},{(rect.Solid ? "1" : "0")}");
      }
    }

    File.WriteAllText(path, sb.ToString());
  }

  public static Tilemap Load(string filePath, string assetsPath, string gameAssetsPath)
  {
    return TilemapParser.LoadFromFile(filePath, assetsPath, gameAssetsPath);
  }

  public List<MapCollision> CreateMapCollisionObjects()
  {
    var mapCollisions = new List<MapCollision>();

    foreach (var rect in CollisionRectangles)
    {
      var mapCollision = new MapCollision(rect.Position, rect.Size);
      mapCollisions.Add(mapCollision);
    }

    return mapCollisions;
  }

  public void SetTile(int layer, int x, int y, int tileIndex)
  {
    if (IsValidLayer(layer) && IsValidPosition(x, y))
    {
      int index = GetTileIndex(x, y);
      Layers[layer][index] = tileIndex;
    }
  }

  public int GetTile(int layer, int x, int y)
  {
    if (IsValidLayer(layer) && IsValidPosition(x, y))
    {
      int index = GetTileIndex(x, y);
      return Layers[layer][index];
    }
    return EmptyTile;
  }

  public void EraseTile(int layer, int x, int y)
  {
    SetTile(layer, x, y, EmptyTile);
  }

  public void FillLayer(int layer, int tileIndex)
  {
    if (IsValidLayer(layer))
    {
      for (int i = 0; i < TotalTiles; i++)
      {
        Layers[layer][i] = tileIndex;
      }
    }
  }

  public void ClearLayer(int layer)
  {
    FillLayer(layer, EmptyTile);
  }

  public void Draw(Vector2 offset, int scale = 1)
  {
    for (int i = 0; i < LayerCount; i++)
    {
      Layers[i].Draw(offset, scale);
    }
  }


  private static bool IsValidLayer(int layer) => layer >= 0 && layer < LayerCount;

  private bool IsValidPosition(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

  private int GetTileIndex(int x, int y) => y * Width + x;

}
