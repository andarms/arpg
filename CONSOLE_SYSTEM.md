# Console Command System

This system provides an extensible console command handler that allows different loops, scenes, and contexts to have their own sets of available commands.

## Architecture Overview

### Core Components

- **`ICommand`**: Interface that all commands must implement
- **`BaseCommand`**: Abstract base class providing common functionality
- **`CommandRegistry`**: Manages command registration and execution
- **`CommandContext`**: Provides execution environment and services
- **`ConsoleManager`**: High-level manager that ties everything together

### Built-in Commands

The system comes with several built-in commands:

- `help` - Show available commands or help for a specific command
- `clear` - Clear console output
- `echo` - Echo text back to console
- `exit` - Exit the application or close console

## Usage Examples

### Basic Command Creation

```csharp
public class MyCommand : BaseCommand
{
    public MyCommand()
        : base("mycommand", "Description of my command", "mycommand <arg>", "alias1", "alias2")
    {
    }

    public override string? Execute(string[] args, ICommandContext context)
    {
        if (!ValidateArgCount(args, 1, context))
            return null;

        var arg = args[0];
        return $"You entered: {arg}";
    }
}
```

### Setting Up Console in a Scene

```csharp
public class MyScene : Scene
{
    private readonly ConsoleManager consoleManager = new();

    public override void Initialize()
    {
        // Register scene-specific commands
        consoleManager.RegisterCommand(new MyCommand());

        // Register services that commands can use
        consoleManager.RegisterService(this);

        // Handle exit requests
        consoleManager.OnExitRequested += () => ScenesController.PopScene();
    }

    void HandleConsoleCommand(string input)
    {
        consoleManager.ExecuteCommand(input);
    }
}
```

### Command with Service Dependency

```csharp
public class PlayerCommand : BaseCommand
{
    public PlayerCommand() : base("player", "Control player", "player <action>") { }

    public override string? Execute(string[] args, ICommandContext context)
    {
        var gameScene = context.GetService<GameplayScene>();
        if (gameScene == null)
        {
            context.OutputError("Player commands only available in game");
            return null;
        }

        // Use the service
        var player = gameScene.GetPlayer();
        // ... command logic

        return "Command executed";
    }
}
```

## Features

### Command History

- Use Up/Down arrows to navigate through command history
- Automatic deduplication of consecutive identical commands

### Auto-completion

- Press Tab for command suggestions
- Shows available commands matching the current input

### Scrolling

- Use Page Up/Down, Home/End to scroll through console output
- Auto-scrolls to bottom when new commands are executed

### Context-Specific Commands

Each scene or context can have its own set of available commands by:

1. Creating a `ConsoleManager` instance
2. Registering the commands relevant to that context
3. Registering services that commands might need

## Extending the System

### Adding New Commands

1. Create a class inheriting from `BaseCommand`:

```csharp
public class CustomCommand : BaseCommand
{
    public CustomCommand() : base("custom", "My custom command") { }

    public override string? Execute(string[] args, ICommandContext context)
    {
        // Implementation here
        return "Success message";
    }
}
```

2. Register it with your console manager:

```csharp
consoleManager.RegisterCommand(new CustomCommand());
```

### Adding Services

Commands can access services through dependency injection:

```csharp
// Register a service
consoleManager.RegisterService<MyService>(myServiceInstance);

// Use in command
var service = context.GetService<MyService>();
```

### Context-Specific Command Sets

Different parts of your application can have different command sets:

**Editor Commands**:

- `create` - Create new assets
- `save` - Save current work
- `load` - Load projects

**Game Commands**:

- `debug` - Toggle debug features
- `player` - Control player state
- `spawn` - Spawn game objects
- `time` - Control game time

**Menu Commands**:

- `options` - Open settings
- `credits` - Show credits

## Integration Points

### Input Handling

The console uses command strings that are parsed automatically:

- Supports quoted arguments: `command "argument with spaces"`
- Handles escaped characters: `command "path\\to\\file"`
- Space-separated arguments: `command arg1 arg2 arg3`

### Output Handling

Commands can provide output in several ways:

- Return a string for normal output
- Use `context.Output()` for immediate output
- Use `context.OutputError()` for error messages
- Use `context.ContextData` to pass data back to the calling code

### Visual Integration

The console UI supports:

- Syntax highlighting (commands, errors, headers)
- Scrollable output with visual scroll indicators
- Command suggestions with visual selection
- Blinking cursor and input field

This system is designed to be lightweight yet powerful, allowing easy extension while maintaining clean separation between different application contexts.
