using System.ComponentModel;
using System.Text;

namespace Arpg.Engine.Tilemaps;

public class TilemapData
{
  private const int LayerCount = 3;
  private const int EmptyTile = -1;

  public int Width { get; private set; }
  public int Height { get; private set; }
  public int TotalTiles => Width * Height;
  public TilemapLayer[] Layers { get; private set; } = new TilemapLayer[LayerCount];
  public Tileset Tileset { get; private set; }
  public string TilesetPath { get; private set; } = string.Empty;

  public TilemapData(int width, int height, Tileset tileset, string tilesetPath = "")
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

    File.WriteAllText(path, sb.ToString());
  }

  public static TilemapData Load(string filePath, string assetsPath, string gameAssetsPath)
  {
    string path = Path.Combine(assetsPath, filePath);
    string[] lines = File.ReadAllLines(path);

    int width = 0, height = 0;
    string tilesetPath = "";
    int currentLayer = -1;
    int currentRowInLayer = 0;

    // Parse metadata
    if (lines.Length > 1)
    {
      string tilesetLine = lines[1].Trim();
      tilesetPath = ParseTilesetPath(tilesetLine);
    }
    if (lines.Length > 2)
    {
      string metadataLine = lines[2].Trim();
      (width, height) = ParseMetadata(metadataLine);
    }

    // Load tileset from the specified path
    string fullTilesetPath = Path.Combine(gameAssetsPath, tilesetPath);
    Tileset tileset = new(LoadTexture(fullTilesetPath), 16, 16);
    TilemapData tilemapData = new(width, height, tileset, tilesetPath);

    // Parse tile data
    for (int i = 3; i < lines.Length; i++)
    {
      string line = lines[i].Trim();

      if (line.StartsWith("Layer"))
      {
        currentLayer = int.Parse(line.Split('=')[1]);
        currentRowInLayer = 0;
        continue;
      }

      if (currentLayer >= 0 && IsValidLayer(currentLayer))
      {
        string[] tileIndices = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (tileIndices.Length > 0)
        {
          for (int x = 0; x < tileIndices.Length; x++)
          {
            int tileIndex = int.Parse(tileIndices[x]);
            tilemapData.SetTile(currentLayer, x, currentRowInLayer, tileIndex);
          }
          currentRowInLayer++;
        }
      }
    }

    return tilemapData;
  }

  private static string ParseTilesetPath(string line)
  {
    if (line.StartsWith("Tileset="))
    {
      return line.Substring("Tileset=".Length);
    }
    return "";
  }

  private static (int width, int height) ParseMetadata(string line)
  {
    int width = 0, height = 0;
    string[] parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);

    foreach (var part in parts)
    {
      var keyValue = part.Split('=', StringSplitOptions.RemoveEmptyEntries);
      if (keyValue.Length == 2)
      {
        if (keyValue[0] == "Width")
          width = int.Parse(keyValue[1]);
        else if (keyValue[0] == "Height")
          height = int.Parse(keyValue[1]);
      }
    }

    return (width, height);
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
