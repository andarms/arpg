namespace Arpg.Engine.Gom;

public class GameObjectComponent
{
  private IReadonlyGameObject? owner;
  protected IReadonlyGameObject Owner => owner ?? throw new InvalidOperationException("State is not attached to any GameObject.");

  public void Attach(IReadonlyGameObject owner)
  {
    this.owner = owner;
  }

  public virtual void Initialize() { }
  public virtual void Update(float dt) { }
  public virtual void Draw(Vector2 position) { }
  public virtual void Terminate() { }
}