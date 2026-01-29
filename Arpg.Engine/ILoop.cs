namespace Arpg.Engine;

public interface ILoop
{
  void Initialize();
  void Update(float deltaTime);
  void Draw();
  void OnExit();

  event Action? OnSwitchRequested;
}