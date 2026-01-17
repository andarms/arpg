namespace Arpg.Game.Gom;

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
  private readonly List<GameObjectComponent> components = [];

  public GameObjectSprite? Sprite { get; set; }


  public virtual void Initialize()
  {
  }

  public virtual void Update(float dt)
  {
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


