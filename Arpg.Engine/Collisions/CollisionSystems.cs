using Arpg.Engine.Gom;

namespace Arpg.Engine.Collisions;

public class CollisionSystem
{
  readonly Dictionary<GameObjectGroup, List<GameObject>> colliderGroups = [];

  public void RegisterGameObject(GameObject gameObject, GameObjectGroup group)
  {
    if (!colliderGroups.ContainsKey(group))
      colliderGroups[group] = [];

    colliderGroups[group].Add(gameObject);
  }

  public void UnregisterGameObject(GameObject gameObject, GameObjectGroup group)
  {
    if (colliderGroups.TryGetValue(group, out var list))
    {
      list.Remove(gameObject);
    }
  }

  // Check if position would collide with any solid colliders
  public bool CheckSolidCollision(Vector2 position, Vector2 size, GameObjectGroup? excludeGroup = null)
  {
    var testBounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

    foreach (var group in colliderGroups.Keys)
    {
      if (excludeGroup != null && group == excludeGroup) continue;

      foreach (var obj in colliderGroups[group])
      {
        var collider = obj.Get<Collider>();
        if (collider != null && collider.Solid && CheckCollisionRecs(testBounds, collider.Bounds))
        {
          return true;
        }
      }
    }

    return false;
  }

  // Check collision with specific group
  public List<GameObject> CheckGroupCollision(Vector2 position, Vector2 size, GameObjectGroup targetGroup)
  {
    var results = new List<GameObject>();
    var testBounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

    if (colliderGroups.TryGetValue(targetGroup, out var objects))
    {
      foreach (var obj in objects)
      {
        var collider = obj.Get<Collider>();
        if (collider != null && CheckCollisionRecs(testBounds, collider.Bounds))
        {
          results.Add(obj);
        }
      }
    }

    return results;
  }

  // X-then-Y movement with solid collision checking using collider bounds
  public Vector2 MoveAndCollide(Vector2 currentPosition, Vector2 targetPosition, Collider collider, GameObjectGroup? excludeGroup = null)
  {
    var movement = targetPosition - currentPosition;
    var finalPosition = currentPosition;

    // Try X movement first - create test bounds with collider offset
    var testX = new Vector2(currentPosition.X + movement.X, currentPosition.Y);
    var testBoundsX = new Rectangle(
      (int)(testX.X + collider.Offset.X),
      (int)(testX.Y + collider.Offset.Y),
      (int)collider.Size.X,
      (int)collider.Size.Y
    );

    if (!CheckSolidCollisionBounds(testBoundsX, excludeGroup))
    {
      finalPosition.X = testX.X;
    }

    // Try Y movement - create test bounds with collider offset
    var testY = new Vector2(finalPosition.X, currentPosition.Y + movement.Y);
    var testBoundsY = new Rectangle(
      (int)(testY.X + collider.Offset.X),
      (int)(testY.Y + collider.Offset.Y),
      (int)collider.Size.X,
      (int)collider.Size.Y
    );

    if (!CheckSolidCollisionBounds(testBoundsY, excludeGroup))
    {
      finalPosition.Y = testY.Y;
    }

    return finalPosition;
  }

  // Helper method to check collision using pre-calculated bounds
  bool CheckSolidCollisionBounds(Rectangle testBounds, GameObjectGroup? excludeGroup = null)
  {
    foreach (var group in colliderGroups.Keys)
    {
      if (excludeGroup != null && group == excludeGroup) continue;

      foreach (var obj in colliderGroups[group])
      {
        var collider = obj.Get<Collider>();
        if (collider != null && collider.Solid && CheckCollisionRecs(testBounds, collider.Bounds))
        {
          return true;
        }
      }
    }

    return false;
  }

  // Process collision events for all non-solid colliders
  public void ProcessCollisionEvents()
  {
    var allColliders = GetAllColliders();

    foreach (var collider in allColliders)
    {
      if (!collider.Solid)
      {
        // Check collisions with all other colliders
        foreach (var other in allColliders)
        {
          if (collider != other && collider.CheckCollision(other))
          {
            collider.RegisterCollision(other);
          }
        }
      }

      collider.UpdateCollisionEvents();
    }
  }

  List<Collider> GetAllColliders()
  {
    var colliders = new List<Collider>();
    foreach (var group in colliderGroups.Values)
    {
      foreach (var obj in group)
      {
        var collider = obj.Get<Collider>();
        if (collider != null)
        {
          colliders.Add(collider);
        }
      }
    }
    return colliders;
  }
}