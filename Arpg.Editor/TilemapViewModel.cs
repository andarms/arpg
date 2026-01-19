
using System.Text;

namespace Arpg.Editor;

public class TilemapViewModel
{
  private const int EmptyTile = -1;
  private const int LayerCount = 3;

  private readonly List<int>[] layers;
  private int width;
  private int height;
  private readonly int totalTiles;

  const string AssetsPath = "C:/Users/andar/apps/hamaka_studio/arpg/Arpg.Game/Assets/Tilemaps";

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

  public void Save()
  {

    string path = Path.Combine(AssetsPath, "map.data");
    StringBuilder sb = new();
    sb.AppendLine("[Tilemap]");
    sb.AppendLine($"Width={width},Height={height}");
    for (int layer = 0; layer < LayerCount; layer++)
    {
      sb.AppendLine($"Layer={layer}");
      for (int y = 0; y < height; y++)
      {
        for (int x = 0; x < width; x++)
        {
          int index = GetTileIndex(x, y);
          int tileIndex = layers[layer][index];
          sb.Append(tileIndex);
          if (x < width - 1)
          {
            sb.Append(' ');
          }
        }
        sb.AppendLine();
      }
    }
    File.WriteAllText(path, sb.ToString());
  }

  public void Load(string filePath)
  {
    try
    {
      string path = Path.Combine(AssetsPath, filePath);
      string[] lines = File.ReadAllLines(path);
      int currentLayer = -1;
      int currentRowInLayer = 0;
      for (int i = 0; i < lines.Length; i++)
      {
        string line = lines[i].Trim();
        if (i == 1)
        {
          SetTileMapMetadata(line);
          continue;
        }
        if (line.StartsWith("Layer"))
        {
          currentLayer = int.Parse(line.Split('=')[1]);
          currentRowInLayer = 0; // Reset row counter for new layer
          continue;
        }
        if (currentLayer >= 0 && IsValidLayer(currentLayer))
        {
          string[] tileIndices = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
          if (tileIndices.Length > 0) // Only process non-empty lines
          {
            for (int x = 0; x < tileIndices.Length; x++)
            {
              int tileIndex = int.Parse(tileIndices[x]);
              SetTile(currentLayer, x, currentRowInLayer, tileIndex);
            }
            currentRowInLayer++; // Increment row counter after processing each row
          }
        }
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error loading tilemap: {ex.Message}");
    }
  }

  private void SetTileMapMetadata(string line)
  {
    string[] parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
    foreach (var part in parts)
    {
      var keyValue = part.Split('=', StringSplitOptions.RemoveEmptyEntries);
      if (keyValue.Length == 2)
      {
        if (keyValue[0] == "Width")
        {
          width = int.Parse(keyValue[1]);
        }
        else if (keyValue[0] == "Height")
        {
          height = int.Parse(keyValue[1]);
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
