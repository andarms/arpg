namespace Arpg.Editor.RoomsEditor;

public class TilesetViewModel
{
  public Texture2D Texture;
  public List<Rectangle> Tiles = [];

  public int SelectedTileIndex = -1;

  public TilesetViewModel(string path)
  {
    Texture = LoadTexture(path);
    const int TILE_SIZE = 16;

    for (int y = 0; y < Texture.Height / TILE_SIZE; y++)
    {
      for (int x = 0; x < Texture.Width / TILE_SIZE; x++)
      {
        Tiles.Add(new Rectangle(x * TILE_SIZE, y * TILE_SIZE, TILE_SIZE, TILE_SIZE));
      }
    }
  }
}
