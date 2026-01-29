namespace Arpg.Engine.Gom;

public class MapCollision : GameObject
{
  public MapCollision(Vector2 position, Vector2 size)
  {
    Position = position;

    // Add a solid collider component
    var collider = new Collider
    {
      Offset = Vector2.Zero, // Relative to GameObject position
      Size = size,
      Solid = true,
      CollisionMask = GameObjectGroup.Player | GameObjectGroup.Enemy // Block player and enemies
    };

    components.Add(collider);
  }
}