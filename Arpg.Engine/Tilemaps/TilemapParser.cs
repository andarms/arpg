using System.Text;

namespace Arpg.Engine.Tilemaps;

public static class TilemapParser
{
  public static Tilemap LoadFromFile(string filePath, string assetsPath, string gameAssetsPath)
  {
    if (string.IsNullOrEmpty(filePath))
    {
      throw new ArgumentNullException(nameof(filePath), "File path cannot be null or empty");
    }
    if (string.IsNullOrEmpty(assetsPath))
    {
      throw new ArgumentNullException(nameof(assetsPath), "Assets path cannot be null or empty");
    }
    if (string.IsNullOrEmpty(gameAssetsPath))
    {
      throw new ArgumentNullException(nameof(gameAssetsPath), "Game assets path cannot be null or empty");
    }

    string path = Path.Combine(assetsPath, filePath);
    path = Path.GetFullPath(path); // Normalize the path

    if (!File.Exists(path))
    {
      throw new FileNotFoundException($"Tilemap file not found: {path}");
    }

    string[] lines = File.ReadAllLines(path);

    if (lines.Length < 3)
    {
      throw new InvalidDataException($"Tilemap file is too short. Expected at least 3 lines, got {lines.Length}");
    }

    int width = 0, height = 0;
    string tilesetPath = "";
    int currentLayer = -1;
    int currentRowInLayer = 0;
    bool inCollisionSection = false;
    List<Tilemap.CollisionRectangle> collisionRectangles = [];

    // Parse metadata
    string tilesetLine = lines[1].Trim();
    tilesetPath = ParseTilesetPath(tilesetLine);
    if (string.IsNullOrEmpty(tilesetPath))
    {
      throw new InvalidDataException($"Invalid tileset path in line: {tilesetLine}");
    }

    string metadataLine = lines[2].Trim();
    (width, height) = ParseMetadata(metadataLine);
    if (width <= 0 || height <= 0)
    {
      throw new InvalidDataException($"Invalid tilemap dimensions: {width}x{height}");
    }

    // Load tileset from the specified path
    string fullTilesetPath = Path.Combine(gameAssetsPath, tilesetPath);
    fullTilesetPath = Path.GetFullPath(fullTilesetPath); // Normalize the path
    if (!File.Exists(fullTilesetPath))
    {
      throw new FileNotFoundException($"Tileset file not found: {fullTilesetPath}");
    }

    Tileset tileset = new(LoadTexture(fullTilesetPath), 16, 16);
    Tilemap tilemapData = new(width, height, tileset, tilesetPath);

    // Parse tile data and collisions
    ParseTileDataAndCollisions(lines, tilemapData, collisionRectangles, ref currentLayer, ref currentRowInLayer, ref inCollisionSection);

    // Add collision rectangles to tilemap
    foreach (var rect in collisionRectangles)
    {
      tilemapData.CollisionRectangles.Add(rect);
    }

    return tilemapData;
  }

  private static void ParseTileDataAndCollisions(string[] lines, Tilemap tilemapData, List<Tilemap.CollisionRectangle> collisionRectangles, ref int currentLayer, ref int currentRowInLayer, ref bool inCollisionSection)
  {
    for (int i = 3; i < lines.Length; i++)
    {
      string line = lines[i].Trim();

      if (line == "[Collisions]" || line == "Collisions")
      {
        inCollisionSection = true;
        currentLayer = -1; // Exit layer parsing mode
        continue;
      }

      if (inCollisionSection)
      {
        ParseCollisionData(line, collisionRectangles);
        continue;
      }

      if (line.StartsWith("Layer"))
      {
        ParseLayerHeader(line, ref currentLayer, ref currentRowInLayer, ref inCollisionSection);
        continue;
      }

      if (currentLayer >= 0 && IsValidLayer(currentLayer) && !inCollisionSection)
      {
        ParseTileDataLine(line, tilemapData, currentLayer, ref currentRowInLayer);
      }
    }
  }

  private static void ParseCollisionData(string line, List<Tilemap.CollisionRectangle> collisionRectangles)
  {
    if (string.IsNullOrWhiteSpace(line))
    {
      return;
    }

    if (line.StartsWith("Rect="))
    {
      var rectData = line.Substring("Rect=".Length).Split(',');
      if (rectData.Length >= 4)
      {
        if (float.TryParse(rectData[0], out float x) &&
            float.TryParse(rectData[1], out float y) &&
            float.TryParse(rectData[2], out float rectWidth) &&
            float.TryParse(rectData[3], out float rectHeight))
        {
          bool solid = rectData.Length >= 5 ? rectData[4] == "1" : true;

          collisionRectangles.Add(new Tilemap.CollisionRectangle(
            new Vector2(x, y),
            new Vector2(rectWidth, rectHeight),
            solid
          ));
        }
      }
    }
    else if (line.StartsWith("{") && line.Contains("x:") && line.Contains("y:") && line.Contains("width:") && line.Contains("height:"))
    {
      // Parse format: {x:0,y:192,width:400,height:32}
      string data = line.Trim('{', '}');
      var pairs = data.Split(',');
      float rectX = 0, rectY = 0, rectW = 0, rectH = 0;

      foreach (var pair in pairs)
      {
        var keyValue = pair.Split(':');
        if (keyValue.Length == 2)
        {
          string key = keyValue[0].Trim();
          if (float.TryParse(keyValue[1].Trim(), out float value))
          {
            switch (key)
            {
              case "x": rectX = value; break;
              case "y": rectY = value; break;
              case "width": rectW = value; break;
              case "height": rectH = value; break;
            }
          }
        }
      }

      // Only add if we have valid dimensions
      if (rectW > 0 && rectH > 0)
      {
        collisionRectangles.Add(new Tilemap.CollisionRectangle(
          new Vector2(rectX, rectY),
          new Vector2(rectW, rectH),
          true // Default to solid for old format
        ));
      }
    }
  }

  private static void ParseLayerHeader(string line, ref int currentLayer, ref int currentRowInLayer, ref bool inCollisionSection)
  {
    if (!line.Contains('='))
    {
      currentLayer = -1;
      return;
    }

    var parts = line.Split('=');
    if (parts.Length < 2)
    {
      currentLayer = -1;
      return;
    }

    string layerValue = parts[1].Trim();
    if (int.TryParse(layerValue, out int parsedLayer) && IsValidLayer(parsedLayer))
    {
      currentLayer = parsedLayer;
      currentRowInLayer = 0;
      inCollisionSection = false;
    }
    else
    {
      // Invalid layer format, skip this line
      currentLayer = -1;
    }
  }

  private static void ParseTileDataLine(string line, Tilemap tilemapData, int currentLayer, ref int currentRowInLayer)
  {
    // Skip lines that don't look like tile data
    if (line.StartsWith("[") || line.StartsWith("{") ||
        line.Contains("Collisions") || line.Contains("=") ||
        string.IsNullOrWhiteSpace(line))
    {
      return;
    }

    // Check if we've exceeded the expected height for this layer
    if (currentRowInLayer >= tilemapData.Height)
    {
      return; // Skip extra rows
    }

    string[] tileIndices = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (tileIndices.Length > 0)
    {
      // Ensure we don't exceed the map width
      int maxX = Math.Min(tileIndices.Length, tilemapData.Width);

      for (int x = 0; x < maxX; x++)
      {
        if (int.TryParse(tileIndices[x], out int tileIndex))
        {
          tilemapData.SetTile(currentLayer, x, currentRowInLayer, tileIndex);
        }
        else
        {
          // Invalid tile index, use empty tile
          tilemapData.SetTile(currentLayer, x, currentRowInLayer, -1);
        }
      }
      currentRowInLayer++;
    }
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
        {
          width = int.Parse(keyValue[1]);
        }
        else if (keyValue[0] == "Height")
        {
          height = int.Parse(keyValue[1]);
        }
      }
    }

    return (width, height);
  }

  private static bool IsValidLayer(int layer) => layer >= 0 && layer < 3;
}