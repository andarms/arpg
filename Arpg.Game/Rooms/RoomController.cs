namespace Arpg.Game.Rooms;

public class RoomController
{
  public Room? ActiveRoom { get; private set; } = null;

  readonly Dictionary<Type, Room> scenes = [];

  public void Register<T>(T scene) where T : Room
  {
    scenes[typeof(T)] = scene;
  }

  public void SwitchTo<T>() where T : Room
  {
    if (!scenes.TryGetValue(typeof(T), out var existingScene))
    {
      return;
    }
    ActiveRoom?.Terminate();
    ActiveRoom = existingScene;
    ActiveRoom.Initialize();
    return;
  }
}
