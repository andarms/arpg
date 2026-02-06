namespace Arpg.Engine;

public class Window(string windowTitle = "ARPG")
{
  private readonly string windowTitle = windowTitle;
  private ILoop? currentLoop;

  public ILoop? GameLoop { get; set; }
  public ILoop? EditorLoop { get; set; }

  public void Run()
  {
    InitializeWindow();
    SwitchToEditorLoop();
    MainLoop();
    currentLoop?.OnExit();
    CloseWindow();
  }

  private void SwitchToGameLoop()
  {
    if (GameLoop == null) return;

    // Current loop handles its own cleanup
    if (currentLoop != null)
    {
      currentLoop.OnSwitchRequested -= SwitchToEditorLoop;
      currentLoop.OnExit();
    }

    currentLoop = GameLoop;
    currentLoop.OnSwitchRequested += SwitchToEditorLoop;
    currentLoop.Initialize();
  }

  private void SwitchToEditorLoop()
  {
#if DEBUG
    if (EditorLoop == null) return;

    // Current loop handles its own cleanup
    if (currentLoop != null)
    {
      currentLoop.OnSwitchRequested -= SwitchToGameLoop;
      currentLoop.OnExit();
    }

    currentLoop = EditorLoop;
    currentLoop.OnSwitchRequested += SwitchToGameLoop;
    currentLoop.Initialize();
#endif
  }

  private void InitializeWindow()
  {
    InitWindow(Settings.Window.Width, Settings.Window.Height, windowTitle);
    SetTargetFPS(Settings.Window.FPS);
    SetExitKey(KeyboardKey.Delete);
  }

  private void MainLoop()
  {
    while (!WindowShouldClose())
    {
      float deltaTime = GetFrameTime();

      currentLoop?.Update(deltaTime);

      BeginDrawing();
      currentLoop?.Draw();
      EndDrawing();
    }
  }
}