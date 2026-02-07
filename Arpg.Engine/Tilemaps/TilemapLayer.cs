using Arpg.Engine.Assets;

namespace Arpg.Engine.Tilemaps;

public class TilemapLayer
{
  private readonly List<int> tiles = [];
  public List<int> Tiles => tiles;

  private readonly Tileset tileset;
  private readonly int mapWidth;
  private readonly int mapHeight;
  public string Name { get; set; }

  public TilemapLayer(int width, int height, string name, Tileset tileset)
  {
    mapWidth = width;
    mapHeight = height;
    Name = name;
    this.tileset = tileset;

    // Initialize with empty tiles
    tiles.AddRange(Enumerable.Repeat(-1, width * height));

    // Load from AssetsManager if available
    if (AssetsManager.Maps.TryGetValue(name, out List<int>? loadedTiles))
    {
      System.Console.WriteLine($"Loaded tilemap '{name}' with {loadedTiles.Count} tiles.");
      for (int i = 0; i < Math.Min(loadedTiles.Count, tiles.Count); i++)
      {
        tiles[i] = loadedTiles[i];
      }
    }
  }

  public int this[int index]
  {
    get
    {
      if (index >= 0 && index < tiles.Count)
        return tiles[index];
      return -1; // Return empty tile for invalid indices
    }
    set
    {
      if (index >= 0 && index < tiles.Count)
        tiles[index] = value;
    }
  }

  public void Draw(Vector2 offset = default, int scale = 1, Color tint = default)
  {
    Color drawColor = tint.Equals(default(Color)) ? Color.White : tint;

    for (int y = 0; y < mapHeight; y++)
    {
      for (int x = 0; x < mapWidth; x++)
      {
        int index = y * mapWidth + x;
        int tileIndex = tiles[index];

        if (tileIndex >= 0)
        {
          Rectangle sourceRect = tileset.CalculateTileRect(tileIndex);
          Vector2 position = new(
            offset.X + x * tileset.TileWidth * scale,
            offset.Y + y * tileset.TileHeight * scale
          );

          Rectangle destRect = new(
            position.X,
            position.Y,
            tileset.TileWidth * scale,
            tileset.TileHeight * scale
          );

          DrawTexturePro(tileset.Texture, sourceRect, destRect, Vector2.Zero, 0.0f, drawColor);
        }
      }
    }
  }
}


