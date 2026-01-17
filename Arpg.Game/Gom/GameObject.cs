namespace Arpg.Game.Gom;

public interface IReadonlyGameObject
{
  Vector2 Position { get; set; }
  IReadOnlyList<GameObjectComponent> Components { get; }
  public GameObjectStateMachine States { get; }
}


public abstract class GameObject : IReadonlyGameObject
{
  public Vector2 Position { get; set; } = Vector2.Zero;
  public GameObjectStateMachine States { get; } = new();
  public IReadOnlyList<GameObjectComponent> Components => components;
  protected readonly List<GameObjectComponent> components = [];

  public GameObjectSprite? Sprite { get; set; }


  public virtual void Initialize()
  {
    // States.Initialize(this);
    components.ForEach(c => c.Attach(this));
  }

  public virtual void Update(float dt)
  {
    // States.Update(this, dt);
    components.ForEach(c => c.Update(dt));
  }

  public virtual void Draw()
  {
    Sprite?.Draw(Position);
  }

  public virtual void Terminate() { }
}


public class UIGameObject : GameObject
{
  public override void Draw()
  {
  }
}


