using Arpg.Engine.Gom;
using Arpg.Engine.Tilemaps;
using Arpg.Game.GameObjects;

namespace Arpg.Game.Rooms;

public class Gameplay : Room
{
  readonly TilemapLayer[] layers = new TilemapLayer[3];
  TilemapData tilemapData;
  Vector2 offset = new(0, 0);

  public Gameplay() : base()
  {
    Add(new Player() { Position = new Vector2(50, 50) }, [GameObjectGroup.Player]);

    tilemapData = TilemapService.LoadFromFile("map.data");

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
    tilemapData.Draw(offset);
    base.Draw();
  }
}