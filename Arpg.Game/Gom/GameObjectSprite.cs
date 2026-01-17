namespace Arpg.Game.Gom;

public class GameObjectSprite
{
  public Texture2D Texture { get; set; }
  public Vector2 Origin { get; set; } = Vector2.Zero;
  public float Rotation { get; set; }
  public Color Tint { get; set; } = Color.White;
  public float Scale { get; set; } = 1.0f;
  public Rectangle Source { get; set; }

  public virtual void Draw(Vector2 position)
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
      Origin,
      Rotation,
      Tint
    );
  }
}
