namespace Arpg.Engine.Scenes;

public abstract class Scene : IDisposable
{
  public Color BackgroundColor { get; set; } = Color.Black;
  public bool IsInitialized { get; private set; } = false;
  public bool IsActive { get; internal set; } = false;

  public virtual void Initialize()
  {
    IsInitialized = true;
  }

  public virtual void Update(float dt) { }

  public virtual void Draw() { }

  public virtual void OnEnter()
  {
    IsActive = true;
  }

  public virtual void OnExit()
  {
    IsActive = false;
  }

  public virtual void OnPause()
  {
    IsActive = false;
  }

  public virtual void OnResume()
  {
    IsActive = true;
  }

  public virtual void Terminate() { }

  public virtual void Dispose()
  {
    Terminate();
    GC.SuppressFinalize(this);
  }
}
