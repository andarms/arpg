using Arpg.Engine.Tilemaps;

namespace Arpg.Editor.RoomsEditor;

public class EditorTilemap(int width, int height, Tileset tileset, string tilesetPath = "") : Tilemap(width, height, tileset, tilesetPath)
{
  private static readonly Color FadeColor = new Color(255, 255, 255, 77); // 30% opacity

  public void DrawWithLayerFading(Vector2 offset, int scale = 1, int selectedLayer = -1)
  {
    if (!GameEditorViewModel.EnableLayerFading)
    {
      // Fall back to base class drawing if fading is disabled
      Draw(offset, scale);
      return;
    }

    // Draw each layer with appropriate opacity
    for (int i = 0; i < Layers.Length; i++)
    {
      Color layerColor = GetLayerColor(i, selectedLayer);
      Layers[i].Draw(offset, scale, layerColor);
    }
  }

  private static Color GetLayerColor(int layerIndex, int selectedLayer)
  {
    if (selectedLayer == (int)TileLayer.Collision || selectedLayer == (int)TileLayer.GameObjects)
    {
      return FadeColor;
    }

    if (selectedLayer == -1 || layerIndex == selectedLayer)
    {
      return Color.White;
    }
    else
    {
      return FadeColor;
    }
  }
}