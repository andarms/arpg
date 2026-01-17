namespace Arpg.Game.Gom;


public class Tileset(Texture2D texture)
{
  public string Name { get; set; } = string.Empty;
  public Texture2D Texture { get; set; } = texture;

  public int TileWidth { get; set; } = 16;
  public int TileHeight { get; set; } = 16;

  public Rectangle CalculateTileRect(int tileIndex)
  {
    int tilesPerRow = Texture.Width / TileWidth;
    int x = tileIndex % tilesPerRow * TileWidth;
    int y = tileIndex / tilesPerRow * TileHeight;
    return new Rectangle(x, y, TileWidth, TileHeight);
  }
}

public class TilemapLayer
{
  readonly List<int> tiles = [];
  public List<int> Tiles => tiles;

  readonly Tileset tileset;

  int mapWidth = 0;
  int mapHeight = 0;

  public TilemapLayer(int width, int height, string mapName, Tileset tileset)
  {
    tiles.AddRange(Enumerable.Repeat(-1, width * height));
    if (Assets.AssetsManager.Maps.TryGetValue(mapName, out List<int>? loadedTiles))
    {
      Console.WriteLine($"Loaded tilemap '{mapName}' with {loadedTiles.Count} tiles.");
      for (int i = 0; i < Math.Min(loadedTiles.Count, tiles.Count); i++)
      {
        tiles[i] = loadedTiles[i];
      }
    }
    mapWidth = width;
    mapHeight = height;
    this.tileset = tileset;
  }

  public void Draw()
  {
    for (int y = 0; y < mapHeight; y++)
    {
      for (int x = 0; x < mapWidth; x++)
      {
        int index = y * mapWidth + x;
        int tileIndex = tiles[index];
        if (tileIndex >= 0)
        {
          Rectangle sourceRect = tileset.CalculateTileRect(tileIndex);
          Rectangle destRect = new(x * tileset.TileWidth, y * tileset.TileHeight, tileset.TileWidth, tileset.TileHeight);
          DrawTextureRec(tileset.Texture, sourceRect, new Vector2(destRect.X, destRect.Y), Color.White);
        }
      }
    }
  }


}


