namespace Arpg.Engine.Gom;

public class Collider : GameObjectComponent
{
  public Vector2 Offset { get; set; } = Vector2.Zero;
  public Vector2 Size { get; set; } = Vector2.One;
  public bool Solid { get; set; } = true;
  public GameObjectGroup CollisionMask { get; set; } = (GameObjectGroup)(-1); // Default: collide with all groups

  // Non-solid collision events
  public Action<Collider>? OnCollisionEnter { get; set; }
  public Action<Collider>? OnCollisionStay { get; set; }
  public Action<Collider>? OnCollisionExit { get; set; }

  // Current frame collisions (for tracking enter/exit events)
  readonly HashSet<Collider> currentCollisions = [];
  readonly HashSet<Collider> previousCollisions = [];

  public Rectangle Bounds => new Rectangle(
      (int)(Owner.Position.X + Offset.X),
      (int)(Owner.Position.Y + Offset.Y),
      (int)Size.X,
      (int)Size.Y
  );

  public bool CheckCollision(Collider other)
  {
    return CheckCollisionRecs(Bounds, other.Bounds);
  }

  public bool CanCollideWith(GameObjectGroup otherGroup)
  {
    return CollisionMask.HasFlag(otherGroup);
  }

  internal void UpdateCollisionEvents()
  {
    if (Solid) return; // Only non-solid colliders trigger events

    // Check for new collisions (OnCollisionEnter)
    foreach (var collision in currentCollisions)
    {
      if (!previousCollisions.Contains(collision))
      {
        OnCollisionEnter?.Invoke(collision);
      }
      else
      {
        OnCollisionStay?.Invoke(collision);
      }
    }

    // Check for ended collisions (OnCollisionExit)
    foreach (var collision in previousCollisions)
    {
      if (!currentCollisions.Contains(collision))
      {
        OnCollisionExit?.Invoke(collision);
      }
    }

    // Swap collision sets for next frame
    previousCollisions.Clear();
    previousCollisions.UnionWith(currentCollisions);
    currentCollisions.Clear();
  }

  internal void RegisterCollision(Collider other)
  {
    if (!Solid) // Only track collisions for non-solid colliders
    {
      currentCollisions.Add(other);
    }
  }

  public override void Debug()
  {
    // Draw collision rectangle
    Color color = Solid ? Color.Red : Color.Yellow;
    DrawRectangleLines(
      (int)(Owner.Position.X + Offset.X),
      (int)(Owner.Position.Y + Offset.Y),
      (int)Size.X,
      (int)Size.Y,
      color
    );
  }
}