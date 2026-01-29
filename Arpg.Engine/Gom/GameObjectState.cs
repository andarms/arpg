namespace Arpg.Engine.Gom;

public class GameObjectState
{
  private GameObject? owner;
  protected GameObject Owner => owner ?? throw new InvalidOperationException("State is not attached to any GameObject.");

  public void Attach(GameObject owner)
  {
    this.owner = owner;
  }

  public virtual void Enter()
  {
  }

  public virtual void Update(float dt)
  {
  }

  public virtual void Exit()
  {
  }

}