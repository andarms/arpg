using System.Text;

namespace Arpg.Engine.Tilemaps;

public static class TilemapParser
{
  public static Tilemap LoadFromFile(string filePath, string assetsPath, string gameAssetsPath)
  {
    string path = Path.Combine(assetsPath, filePath);
    string[] lines = File.ReadAllLines(path);

    int width = 0, height = 0;
    string tilesetPath = "";
    int currentLayer = -1;
    int currentRowInLayer = 0;
    bool inCollisionSection = false;
    List<Tilemap.CollisionRectangle> collisionRectangles = [];

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
    if (line.StartsWith("Rect="))
    {
      var rectData = line.Substring("Rect=".Length).Split(',');
      if (rectData.Length >= 5)
      {
        float x = float.Parse(rectData[0]);
        float y = float.Parse(rectData[1]);
        float rectWidth = float.Parse(rectData[2]);
        float rectHeight = float.Parse(rectData[3]);
        bool solid = rectData[4] == "1";

        collisionRectangles.Add(new Tilemap.CollisionRectangle(
          new Vector2(x, y),
          new Vector2(rectWidth, rectHeight),
          solid
        ));
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
          float value = float.Parse(keyValue[1].Trim());

          switch (key)
          {
            case "x": rectX = value; break;
            case "y": rectY = value; break;
            case "width": rectW = value; break;
            case "height": rectH = value; break;
          }
        }
      }

      collisionRectangles.Add(new Tilemap.CollisionRectangle(
        new Vector2(rectX, rectY),
        new Vector2(rectW, rectH),
        true // Default to solid for old format
      ));
    }
  }

  private static void ParseLayerHeader(string line, ref int currentLayer, ref int currentRowInLayer, ref bool inCollisionSection)
  {
    string layerValue = line.Split('=')[1];
    if (int.TryParse(layerValue, out int parsedLayer))
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

    string[] tileIndices = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (tileIndices.Length > 0)
    {
      for (int x = 0; x < tileIndices.Length; x++)
      {
        if (int.TryParse(tileIndices[x], out int tileIndex))
        {
          tilemapData.SetTile(currentLayer, x, currentRowInLayer, tileIndex);
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
          width = int.Parse(keyValue[1]);
        else if (keyValue[0] == "Height")
          height = int.Parse(keyValue[1]);
      }
    }

    return (width, height);
  }

  private static bool IsValidLayer(int layer) => layer >= 0 && layer < 3;
}