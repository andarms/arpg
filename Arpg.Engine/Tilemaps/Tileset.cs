namespace Arpg.Engine.Tilemaps;

public class Tileset(Texture2D texture, int tileWidth, int tileHeight)
{
  public string Name { get; set; } = string.Empty;
  public Texture2D Texture { get; set; } = texture;

  public int TileWidth => tileWidth;
  public int TileHeight => tileHeight;

  public Rectangle CalculateTileRect(int tileIndex)
  {
    int tilesPerRow = Texture.Width / tileWidth;
    int x = tileIndex % tilesPerRow * tileWidth;
    int y = tileIndex / tilesPerRow * tileHeight;
    return new Rectangle(x, y, tileWidth, tileHeight);
  }
}


