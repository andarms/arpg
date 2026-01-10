using Arpg.Game.Gom;

namespace Arpg.Game.Scenes;

public class Scene
{
  public Color BackgroundColor { get; set; } = Color.Black;

  readonly List<GameObject> objects = [];

  public IReadOnlyList<GameObject> Objects => objects;

  public virtual void Initialize()
  {
    objects.ForEach(go => go.Initialize());
  }

  public virtual void Update(float dt)
  {
    objects.ForEach(go => go.Update(dt));
  }

  public virtual void Draw()
  {
    ClearBackground(BackgroundColor);
    objects.ForEach(go => go.Draw());
  }

  public virtual void Terminate()
  {
    objects.ForEach(go => go.Terminate());
  }


  public void Add(GameObject gameObject)
  {
    objects.Add(gameObject);
  }

  public void Remove(GameObject gameObject)
  {
    objects.Remove(gameObject);
  }
}