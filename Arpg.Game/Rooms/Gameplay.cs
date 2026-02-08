using Arpg.Engine.Gom;
using Arpg.Engine.Tilemaps;
using Arpg.Game.GameObjects;

namespace Arpg.Game.Rooms;

public class Gameplay : Room
{
  Tilemap tilemapData;
  Vector2 offset = new(0, 0);

  public Gameplay() : base()
  {
    Add(new Player() { Position = new Vector2(50, 50) }, [GameObjectGroup.Player]);

    tilemapData = TilemapService.LoadFromFile("map.room");
    Game.Viewport.SetLimits(tilemapData.Width, tilemapData.Height);

    // Load collision objects from tilemap data
    var mapCollisions = tilemapData.CreateMapCollisionObjects();
    foreach (var mapCollision in mapCollisions)
    {
      Add(mapCollision, [GameObjectGroup.Obstacle]);
    }
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    if (IsKeyPressed(KeyboardKey.F1))
    {
      Game.DebugMode = !Game.DebugMode;
    }
  }

  public override void Draw()
  {
    // Draw layers individually to allow game objects between layers
    tilemapData.Layer1.Draw(offset);
    tilemapData.Layer2.Draw(offset);
    base.Draw(); // Draw game objects
    tilemapData.Layer3.Draw(offset);
  }
}