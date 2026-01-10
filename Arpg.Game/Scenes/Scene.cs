namespace Arpg.Game.Scenes;

public class Scene
{
  public Color BackgroundColor { get; set; } = Color.Black;

  public virtual void Initialize() { }

  public virtual void Update(float dt) { }

  public virtual void Draw()
  {
    ClearBackground(BackgroundColor);
  }

  public virtual void Terminate() { }
}