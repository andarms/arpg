using Arpg.Engine.Scenes;
using Arpg.Engine.GameConsole;
using Arpg.Editor.GameConsole.Commands;

namespace Arpg.Editor.GameConsole;

public class ConsoleScene : Scene
{
  readonly InputField inputField = new();
  readonly ConsoleManager consoleManager = new();
  private int scrollOffset = 0;
  private readonly int maxVisibleLines = 24;

  public override void Initialize()
  {
    base.Initialize();
    inputField.OnSubmit += HandleCommand;
    inputField.SetConsoleManager(consoleManager);

    // Register editor-specific commands
    RegisterEditorCommands();

    // Setup console manager events
    consoleManager.OnExitRequested += () => ScenesController.PopScene();
    consoleManager.OnLinesAdded += HandleAutoScroll;

    // Welcome message
    consoleManager.AddOutputLine("=== Console ===");
    consoleManager.AddOutputLine("Type 'help' for available commands.");
    consoleManager.AddOutputLine("");
  }

  void HandleCommand(string command)
  {
    consoleManager.ExecuteCommand(command);
  }

  void HandleAutoScroll(int linesAdded)
  {
    // Auto-scroll to show new content
    var outputCount = consoleManager.OutputLines.Count;
    if (outputCount > maxVisibleLines)
    {
      scrollOffset = outputCount - maxVisibleLines;
    }
  }

  void RegisterEditorCommands()
  {
    consoleManager.RegisterCommand(new LoadCommand());
    consoleManager.RegisterCommand(new CreateCommand());
    consoleManager.RegisterCommand(new MenuCommand());

    consoleManager.RegisterService(new RoomsService());
  }

  public override void Update(float dt)
  {
    base.Update(dt);

    // Handle console toggle
    if (IsKeyPressed(Constants.ConsoleToggleKey))
    {
      ScenesController.PopScene();
    }

    // Handle scrolling through output with mouse wheel or arrow keys
    var outputCount = consoleManager.OutputLines.Count;
    if (outputCount > maxVisibleLines)
    {
      float mouseWheelMove = GetMouseWheelMove();
      if (mouseWheelMove != 0)
      {
        scrollOffset = Math.Max(0, Math.Min(outputCount - maxVisibleLines, scrollOffset - (int)(mouseWheelMove * 3)));
      }

      if (IsKeyPressed(KeyboardKey.Up) && IsKeyDown(KeyboardKey.LeftControl))
      {
        scrollOffset = Math.Max(0, scrollOffset - 1);
      }
      else if (IsKeyPressed(KeyboardKey.Down) && IsKeyDown(KeyboardKey.LeftControl))
      {
        scrollOffset = Math.Min(outputCount - maxVisibleLines, scrollOffset + 1);
      }
      else if (IsKeyPressed(KeyboardKey.Home) && IsKeyDown(KeyboardKey.LeftControl))
      {
        scrollOffset = 0;
      }
      else if (IsKeyPressed(KeyboardKey.End) && IsKeyDown(KeyboardKey.LeftControl))
      {
        scrollOffset = outputCount - maxVisibleLines;
      }
    }

    inputField.Update(dt);
  }

  public override void Draw()
  {
    base.Draw();
    DrawRectangle(0, 0, GetScreenWidth(), GetScreenHeight(), new Color(0, 0, 0, 220));

    DrawConsoleOutput();
    DrawScrollIndicator();
    inputField.Draw();
  }

  void DrawConsoleOutput()
  {
    var outputLines = consoleManager.OutputLines;
    var startY = Constants.Padding;
    var lineHeight = 24;
    var startIndex = Math.Max(0, scrollOffset);
    var endIndex = Math.Min(outputLines.Count, startIndex + maxVisibleLines);

    for (int i = startIndex; i < endIndex; i++)
    {
      var line = outputLines[i];
      var y = startY + (i - startIndex) * lineHeight;

      // Color code different types of output
      var color = Color.White;
      if (line.StartsWith(">"))
      {
        color = Color.Yellow; // Input commands
      }
      else if (line.StartsWith("ERROR:"))
      {
        color = Color.Red; // Errors
      }
      else if (line.StartsWith("==="))
      {
        color = new Color(0, 255, 255, 255); // Headers (Cyan)
      }

      DrawTextEx(Constants.DefaultFont, line, new Vector2(Constants.Padding, y), 20, 1, color);
    }
  }

  void DrawScrollIndicator()
  {
    var outputCount = consoleManager.OutputLines.Count;
    if (outputCount <= maxVisibleLines)
    {
      return;
    }

    var screenHeight = GetScreenHeight();
    var inputHeight = 50;
    var availableHeight = screenHeight - inputHeight - Constants.Padding * 2;

    // Draw scroll bar background
    var scrollBarX = GetScreenWidth() - 20;
    var scrollBarY = Constants.Padding;
    DrawRectangle(scrollBarX, scrollBarY, 10, availableHeight, new Color(50, 50, 50, 180));

    // Draw scroll thumb
    var thumbHeight = Math.Max(20, availableHeight * maxVisibleLines / outputCount);
    var thumbY = scrollBarY + (availableHeight - thumbHeight) * scrollOffset / (outputCount - maxVisibleLines);
    DrawRectangle(scrollBarX, thumbY, 10, thumbHeight, new Color(150, 150, 150, 200));
  }
}