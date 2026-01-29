namespace Arpg.Engine.Gom;

public interface IReadonlyGameObject
{
  Vector2 Position { get; }
  IReadOnlyList<GameObjectComponent> Components { get; }
  public GameObjectStateMachine States { get; }
}


public abstract class GameObject : IReadonlyGameObject
{
  public Vector2 Position { get; set; } = Vector2.Zero;
  public GameObjectStateMachine States { get; } = new();
  public IReadOnlyList<GameObjectComponent> Components => components;
  protected readonly List<GameObjectComponent> components = [];
  Dictionary<Type, GameObjectComponent?> componentCache { get; } = [];

  public virtual void Initialize()
  {
    States.Attach(this);
    components.ForEach(c => c.Attach(this));
  }

  public virtual void Update(float dt)
  {
    States.ActiveState?.Update(dt);
    components.ForEach(c => c.Update(dt));
  }

  public virtual void Draw()
  {
    components.ForEach(c => c.Draw(Position));
  }

  public virtual void Debug()
  {
    DrawCircleV(Position, 2, Color.Green);
    components.ForEach(c => c.Debug());
  }

  public T? Get<T>() where T : GameObjectComponent
  {
    var type = typeof(T);
    if (!componentCache.TryGetValue(type, out var cached))
    {
      cached = components.FirstOrDefault(c => c is T);
      componentCache[type] = cached;
    }
    return cached as T;
  }

  public void Add(GameObjectComponent component)
  {
    components.Add(component);
    componentCache.Clear(); // Clear cache when adding new components
  }



  public virtual void Terminate() { }
}
