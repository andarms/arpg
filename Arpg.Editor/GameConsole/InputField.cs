using Arpg.Engine;
using Arpg.Engine.GameConsole;

namespace Arpg.Editor.GameConsole;

public class InputField
{
  Vector2 position = new(Constants.Padding, GetScreenHeight() - Constants.Padding4);
  Rectangle Bound = new(Constants.Padding, GetScreenHeight() - Constants.Padding4, GetScreenWidth() - Constants.Padding2, 40);

  string text = "";
  private float cursorTimer = 0f;
  private List<string> commandHistory = new();
  private int historyIndex = -1;
  private string? currentInput = null;
  private List<string> suggestions = new();
  private int suggestionIndex = -1;
  private bool showSuggestions = false;
  private ConsoleManager? consoleManager;

  public Action<string>? OnSubmit;

  public void SetConsoleManager(ConsoleManager manager)
  {
    consoleManager = manager;
  }

  public void Update(float dt)
  {
    cursorTimer += dt;

    // Handle tab for auto-completion
    if (IsKeyPressed(KeyboardKey.Tab) && consoleManager != null)
    {
      HandleAutoComplete();
    }

    // Handle command history navigation with Page Up/Page Down
    if (IsKeyPressed(KeyboardKey.Up))
    {
      NavigateHistory(-1);
    }
    else if (IsKeyPressed(KeyboardKey.Down))
    {
      NavigateHistory(1);
    }

    // Handle up/down arrows for suggestion selection
    if (IsKeyPressed(KeyboardKey.Up) && showSuggestions)
    {
      suggestionIndex = Math.Max(-1, suggestionIndex - 1);
      if (suggestionIndex >= 0)
      {
        text = suggestions[suggestionIndex];
      }
    }
    else if (IsKeyPressed(KeyboardKey.Down) && showSuggestions)
    {
      suggestionIndex = Math.Min(suggestions.Count - 1, suggestionIndex + 1);
      if (suggestionIndex >= 0)
      {
        text = suggestions[suggestionIndex];
      }
    }

    // Handle escape to cancel suggestions or clear input
    if (IsKeyPressed(KeyboardKey.Escape))
    {
      if (showSuggestions)
      {
        showSuggestions = false;
        suggestionIndex = -1;
      }
      else
      {
        text = "";
        ResetHistory();
      }
    }

    // Handle backspace
    if (IsKeyPressed(KeyboardKey.Backspace) && text.Length > 0)
    {
      text = text[..^1];
      ResetHistory();
      UpdateSuggestions();
    }

    // Handle enter key
    if (IsKeyPressed(KeyboardKey.Enter) && text.Length > 0)
    {
      OnSubmit?.Invoke(text);

      // Add to history if it's not empty and not the same as the last command
      if (!string.IsNullOrWhiteSpace(text) &&
          (commandHistory.Count == 0 || commandHistory[^1] != text))
      {
        commandHistory.Add(text);
        // Keep history manageable
        if (commandHistory.Count > 50)
        {
          commandHistory.RemoveAt(0);
        }
      }

      text = "";
      ResetHistory();
      showSuggestions = false;
    }

    // Handle character input
    int key = GetCharPressed();
    while (key > 0)
    {
      if (key >= 32 && key <= 126) // Printable ASCII range
      {
        text += (char)key;
        ResetHistory();
        UpdateSuggestions();
      }
      key = GetCharPressed(); // Get next character if any
    }
  }


  public void Draw()
  {
    DrawRectangleRec(Bound, new Color(30, 30, 30, 180));
    DrawTextEx(Constants.DefaultFont, "$", new Vector2(position.X + 10, position.Y), 32, 2, Color.White);
    DrawTextEx(Constants.DefaultFont, text, new Vector2(position.X + 40, position.Y), 32, 2, Color.White);

    // Draw blinking cursor
    if ((cursorTimer % 1.0f) < 0.5f) // Blink every 0.5 seconds
    {
      Vector2 textSize = MeasureTextEx(Constants.DefaultFont, text, 32, 2);
      DrawTextEx(Constants.DefaultFont, "_", new Vector2(position.X + 40 + textSize.X, position.Y), 32, 2, Color.White);
    }

    // Draw suggestions
    if (showSuggestions && suggestions.Count > 0)
    {
      DrawSuggestions();
    }
  }

  void DrawSuggestions()
  {
    var suggestionY = position.Y - 10;
    var maxSuggestions = Math.Min(5, suggestions.Count);
    var suggestionHeight = 25;

    // Draw background for suggestions
    var bgHeight = maxSuggestions * suggestionHeight + 10;
    DrawRectangle((int)position.X, (int)(suggestionY - bgHeight),
                 (int)Bound.Width, bgHeight,
                 new Color(20, 20, 20, 200));

    // Draw suggestions
    for (int i = 0; i < maxSuggestions; i++)
    {
      var suggestion = suggestions[i];
      var y = suggestionY - (maxSuggestions - i) * suggestionHeight;
      var color = i == suggestionIndex ? Color.Yellow : Color.Gray;

      DrawTextEx(Constants.DefaultFont, suggestion,
                new Vector2(position.X + 10, y), 20, 1, color);
    }
  }

  void HandleAutoComplete()
  {
    if (consoleManager == null)
    {
      return;
    }

    UpdateSuggestions();

    if (suggestions.Count == 1)
    {
      // Auto-complete with the single suggestion
      text = suggestions[0] + " ";
      showSuggestions = false;
    }
    else if (suggestions.Count > 1)
    {
      showSuggestions = true;
      suggestionIndex = 0;
    }
  }

  void UpdateSuggestions()
  {
    if (consoleManager == null || string.IsNullOrWhiteSpace(text))
    {
      suggestions.Clear();
      showSuggestions = false;
      return;
    }

    // Get command suggestions
    var parts = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (parts.Length <= 1)
    {
      // Suggest commands
      var prefix = parts.Length > 0 ? parts[0] : "";
      suggestions = consoleManager.GetCommandSuggestions(prefix).Take(10).ToList();
    }
    else
    {
      // For now, don't suggest arguments
      suggestions.Clear();
    }

    showSuggestions = suggestions.Count > 0;
    suggestionIndex = -1;
  }

  void NavigateHistory(int direction)
  {
    if (commandHistory.Count == 0)
    {
      return;
    }

    if (historyIndex == -1 && direction == -1)
    {
      // Start browsing history from the end
      currentInput = text;
      historyIndex = commandHistory.Count - 1;
      text = commandHistory[historyIndex];
    }
    else if (historyIndex >= 0)
    {
      historyIndex += direction;

      if (historyIndex < 0)
      {
        historyIndex = -1;
        text = currentInput ?? "";
      }
      else if (historyIndex >= commandHistory.Count)
      {
        historyIndex = commandHistory.Count - 1;
      }
      else
      {
        text = commandHistory[historyIndex];
      }
    }
  }

  void ResetHistory()
  {
    historyIndex = -1;
    currentInput = null;
  }
}