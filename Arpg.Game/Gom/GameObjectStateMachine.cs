namespace Arpg.Game.Gom;

public class GameObjectStateMachine
{
  readonly Dictionary<Type, GameObjectState> states = [];
  public GameObjectState? ActiveState { get; private set; }

  public void Attach(GameObject owner)
  {
    foreach (var state in states.Values)
    {
      state.Attach(owner);
    }
  }

  public void Register<T>(T state) where T : GameObjectState
  {
    states[typeof(T)] = state;
  }

  public void Transition<T>() where T : GameObjectState
  {
    if (!states.TryGetValue(typeof(T), out var existingState))
    {
      return;
    }
    ActiveState?.Exit();
    ActiveState = existingState;
    ActiveState.Enter();
    return;
  }


  public void SetInitial<T>() where T : GameObjectState
  {
    if (!states.TryGetValue(typeof(T), out var existingState))
    {
      return;
    }
    ActiveState = existingState;
    ActiveState.Enter();
    return;
  }
}