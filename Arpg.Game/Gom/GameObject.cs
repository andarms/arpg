namespace Arpg.Game.Gom;

public abstract class GameObject
{
  public Vector2 Position { get; set; } = Vector2.Zero;

  public virtual void Initialize() { }

  public virtual void Update(float dt) { }

  public virtual void Draw() { }

  public virtual void Terminate() { }
}
