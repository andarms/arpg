
namespace Arpg.Editor;

public class TilemapViewModel
{
  private const int EmptyTile = -1;
  private const int LayerCount = 3;

  private readonly List<int>[] layers;
  private readonly int width;
  private readonly int height;
  private readonly int totalTiles;

  public TilemapViewModel(int width, int height)
  {
    this.width = width;
    this.height = height;
    totalTiles = width * height;

    layers = new List<int>[LayerCount];
    for (int i = 0; i < LayerCount; i++)
    {
      layers[i] = [.. new int[totalTiles]];
      for (int j = 0; j < totalTiles; j++)
      {
        layers[i][j] = EmptyTile;
      }
    }
  }

  public void SetTile(int layer, int x, int y, int tileIndex)
  {
    if (IsValidLayer(layer) && IsValidPosition(x, y))
    {
      int index = GetTileIndex(x, y);
      layers[layer][index] = tileIndex;
    }
  }

  public void EraseTile(int layer, int x, int y)
  {
    SetTile(layer, x, y, EmptyTile);
  }

  public void FillLayer(int layer, int tileIndex)
  {
    if (IsValidLayer(layer))
    {
      for (int i = 0; i < totalTiles; i++)
      {
        layers[layer][i] = tileIndex;
      }
    }
  }

  public void Draw()
  {
    for (int y = 0; y < height; y++)
    {
      for (int x = 0; x < width; x++)
      {
        int index = GetTileIndex(x, y);
        Rectangle destination = GetTileDestination(x, y);

        for (int layer = 0; layer < LayerCount; layer++)
        {
          int tileIndex = layers[layer][index];
          if (tileIndex >= 0)
          {
            DrawTile(tileIndex, destination);
          }
        }
      }
    }
  }

  private static bool IsValidLayer(int layer) => layer >= 0 && layer < LayerCount;

  private bool IsValidPosition(int x, int y) => x >= 0 && x < width && y >= 0 && y < height;

  private int GetTileIndex(int x, int y) => y * width + x;

  private static Rectangle GetTileDestination(int x, int y)
  {
    return new(
      Settings.Padding + x * Settings.ScaledTileSize,
      Settings.Padding + y * Settings.ScaledTileSize,
      Settings.ScaledTileSize,
      Settings.ScaledTileSize
    );
  }

  private static void DrawTile(int tileIndex, Rectangle destination)
  {
    Rectangle source = GameEditorViewModel.Tileset.Tiles.ElementAt(tileIndex);
    DrawTexturePro(Settings.TileSet, source, destination, Vector2.Zero, 0f, Color.White);
  }
}
