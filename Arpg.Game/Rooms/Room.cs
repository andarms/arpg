using Arpg.Engine.Gom;

namespace Arpg.Game.Rooms;

public class Room
{
  public Color BackgroundColor { get; set; } = Color.Black;

  readonly List<GameObject> objects = [];
  readonly Dictionary<GameObjectGroup, List<GameObject>> groups = [];

  public IReadOnlyList<GameObject> Objects => objects;
  public IReadOnlyDictionary<GameObjectGroup, List<GameObject>> Groups => groups;

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
    IEnumerable<GameObject> sortedObjects = objects.OrderBy(go => go.Position.Y);
    foreach (GameObject go in sortedObjects)
    {
      go.Draw();
      if (Game.DebugMode)
      {
        go.Debug();
      }
    }
  }

  public virtual void Terminate()
  {
    objects.ForEach(go => go.Terminate());
  }

  public void Add(GameObject gameObject, List<GameObjectGroup> groups)
  {
    foreach (var group in groups)
    {
      if (!this.groups.TryGetValue(group, out List<GameObject>? groupObjects))
      {
        groupObjects = [];
        this.groups[group] = groupObjects;
      }
      groupObjects.Add(gameObject);

      // Register with collision system if it has a collider
      if (gameObject.Get<Collider>() != null)
      {
        Game.Collisions.RegisterGameObject(gameObject, group);
      }
    }
    objects.Add(gameObject);
  }

  public void Add(GameObject gameObject)
  {
    objects.Add(gameObject);

    // For objects added without groups, register with Obstacle group if they have colliders
    if (gameObject.Get<Collider>() != null)
    {
      Game.Collisions.RegisterGameObject(gameObject, GameObjectGroup.Obstacle);
    }
  }

  public void Remove(GameObject gameObject)
  {
    objects.Remove(gameObject);
    foreach (var kvp in groups)
    {
      if (kvp.Value.Remove(gameObject))
      {
        // Unregister from collision system if it has a collider
        if (gameObject.Get<Collider>() != null)
        {
          Game.Collisions.UnregisterGameObject(gameObject, kvp.Key);
        }
      }
    }
  }

  public void Clear()
  {
    objects.Clear();
    groups.Clear();
  }

  public List<GameObject> Get(GameObjectGroup group)
  {
    if (groups.TryGetValue(group, out List<GameObject>? groupObjects))
    {
      return groupObjects;
    }
    return [];
  }
}