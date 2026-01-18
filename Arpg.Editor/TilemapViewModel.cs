
namespace Arpg.Editor;

public class TilemapViewModel
{
  readonly List<int> layer1 = [];
  readonly List<int> layer2 = [];
  readonly List<int> layer3 = [];

  readonly int width;
  readonly int height;

  readonly int tilesPerRow;

  public TilemapViewModel(int width, int height)
  {
    this.width = width;
    this.height = height;
    tilesPerRow = width / Settings.TileSize;

    for (int i = 0; i < width * height; i++)
    {
      layer1.Add(-1);
      layer2.Add(-1);
      layer3.Add(-1);
    }
  }


  public void SetTile(int layer, int x, int y, int tileIndex)
  {
    int index = y * width + x;
    switch (layer)
    {
      case 0:
        layer1[index] = tileIndex;
        break;
      case 1:
        layer2[index] = tileIndex;
        break;
      case 2:
        layer3[index] = tileIndex;
        break;
      default:
        break;
    }
  }

  public void EraseTile(int layer, int x, int y)
  {
    SetTile(layer, x, y, -1);
  }


  public void FillLayer(int layer, int tileIndex)
  {
    for (int y = 0; y < height; y++)
    {
      for (int x = 0; x < width; x++)
      {
        SetTile(layer, x, y, tileIndex);
      }
    }
  }


  public void Draw()
  {
    for (int y = 0; y < height; y++)
    {
      for (int x = 0; x < width; x++)
      {
        int index = y * width + x;

        int tile1 = layer1[index];
        if (tile1 >= 0)
        {
          Rectangle source = GameEditorViewModel.Tileset.Tiles.ElementAt(tile1);
          Rectangle destination = new(Settings.Padding + x * Settings.ScaledTileSize, Settings.Padding + y * Settings.ScaledTileSize, Settings.ScaledTileSize, Settings.ScaledTileSize);
          DrawTexturePro(Settings.TileSet,
            source,
            destination,
            Vector2.Zero,
            0f,
            Color.White);
        }

        int tile2 = layer2[index];
        if (tile2 >= 0)
        {
          Rectangle destination = new(Settings.Padding + x * Settings.ScaledTileSize, Settings.Padding + y * Settings.ScaledTileSize, Settings.ScaledTileSize, Settings.ScaledTileSize);
          Rectangle source = GameEditorViewModel.Tileset.Tiles.ElementAt(tile2);
          DrawTexturePro(Settings.TileSet,
            source,
            destination,
            Vector2.Zero,
            0f,
            Color.White);
        }

        int tile3 = layer3[index];
        if (tile3 >= 0)
        {
          Rectangle destination = new(Settings.Padding + x * Settings.ScaledTileSize, Settings.Padding + y * Settings.ScaledTileSize, Settings.ScaledTileSize, Settings.ScaledTileSize);
          Rectangle source = GameEditorViewModel.Tileset.Tiles.ElementAt(tile3);
          DrawTexturePro(Settings.TileSet,
            source,
            destination,
            Vector2.Zero,
            0f,
            Color.White);
        }
      }
    }
  }
}
