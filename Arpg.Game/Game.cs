using Arpg.Game.Rooms;
using Arpg.Game.Utils;

namespace Arpg.Game;

public static class Game
{
  public static RoomController Rooms { get; } = new();
  public static RandomNumberGenerator Rng { get; } = new();
  public static Room ActiveRoom => Rooms.ActiveRoom ?? throw new InvalidOperationException("No active room.");

  public static void Initialize()
  {
    Rooms.Register(new Gameplay());
    Rooms.SwitchTo<Gameplay>();
  }

  public static void Update(float dt)
  {
    Rooms.ActiveRoom?.Update(dt);
  }
  public static void Draw()
  {
    Rooms.ActiveRoom?.Draw();
  }

  public static void Terminate()
  {
    Rooms.ActiveRoom?.Terminate();
  }
}
