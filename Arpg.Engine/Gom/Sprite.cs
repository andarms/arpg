namespace Arpg.Engine.Gom;

public class Sprite : GameObjectComponent
{
  public Texture2D Texture { get; set; }
  public Vector2 Anchor { get; set; } = Vector2.Zero;
  public float Rotation { get; set; }
  public Color Tint { get; set; } = Color.White;
  public float Scale { get; set; } = 1.0f;
  public Rectangle Source { get; set; }

  public override void Draw(Vector2 position)
  {
    DrawTexturePro(
      Texture,
      Source,
      new Rectangle(
        position.X,
        position.Y,
        Source.Width * Scale,
        Source.Height * Scale
      ),
      Anchor,
      Rotation,
      Tint
    );
  }
}


