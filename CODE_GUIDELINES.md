# C# Code Guidelines

Based on the existing codebase and specific preferences, here are the coding standards to follow:

## Variable Naming

### ❌ DON'T: Use underscore prefix on private variables

```csharp
// DON'T
private int _currentPage;
private readonly List<Component> _components;
```

### ✅ DO: Use camelCase for private variables without prefix

```csharp
// DO
private int currentPage;
private readonly List<GameObjectComponent> components;
```

## Variable Declaration in Methods

### ❌ DON'T: Declare variables in methods for values that don't change

```csharp
// DON'T
public void Update()
{
    const int tileSize = 32; // This should be a class constant
    int screenWidth = GetScreenWidth(); // If this doesn't change during method
    // ... rest of method
}
```

### ✅ DO: Use class constants or readonly fields for unchanging values

```csharp
// DO
private const int TileSize = 32;
private readonly int screenWidth = GetScreenWidth();

public void Update()
{
    // Use TileSize and screenWidth directly
}
```

### ✅ DO: Declare variables in methods only when they change or are computed

```csharp
// DO
public void CheckTileCollision()
{
    Vector2 mousePosition = GetMousePosition(); // Changes each call
    int calculatedIndex = currentPage * TILES_PER_PAGE + i; // Computed value
}
```

## Naming Conventions

### Classes and Public Members

- **PascalCase** for classes, properties, methods, and public fields

```csharp
public class TilesetPanel
public Vector2 Position { get; set; }
public void Update()
```

### Private and Local Variables

- **camelCase** for private fields and local variables

```csharp
private int currentPage = 0;
private readonly int gridRows = 6;
```

### Constants

- **SCREAMING_SNAKE_CASE** for public constants
- **PascalCase** for private constants

```csharp
private const int TILES_PER_PAGE = 156;  // Public-like constant
private const int LayerCount = 3;        // Internal constant
```

## Fields and Properties

### ✅ DO: Use readonly for fields that don't change after construction

```csharp
private readonly int gridRows = 6;
private readonly int gridCols = panelTilesWide;
```

### ✅ DO: Initialize collections inline when possible

```csharp
protected readonly List<GameObjectComponent> components = [];
Dictionary<Type, GameObjectComponent?> componentCache { get; } = [];
```

### ✅ DO: Use auto-properties for simple properties

```csharp
public Vector2 Position { get; set; } = Vector2.Zero;
public GameObjectStateMachine States { get; } = new();
```

## Static Classes and Members

### ✅ DO: Use static classes for utility/service classes

```csharp
public static class GameEditorViewModel
public static class Settings
```

### ✅ DO: Use static readonly for configuration values

```csharp
public static readonly int WindowWidth = 1280;
public static readonly float ZOOM = 3.0f;
```

## Method Structure

### ✅ DO: Use early returns for validation/edge cases

```csharp
public void Update()
{
    if (IsMouseButtonPressed(MouseButton.Left))
    {
        Vector2 mousePosition = GetMousePosition();

        if (CheckCollisionPointRec(mousePosition, pageUpButton) && CanPageUp)
        {
            currentPage--;
            return; // Early return
        }

        // Continue with main logic...
    }
}
```

### ✅ DO: Keep methods focused and single-purpose

- Each method should do one thing well
- Extract complex logic into separate methods

## Collections and LINQ

### ✅ DO: Use collection expressions when available

```csharp
List<GameObjectComponent> components = [];
```

### ✅ DO: Use LINQ for data transformation

```csharp
public IEnumerable<Rectangle> TilesOnPage =>
    GameEditorViewModel.Tileset.Tiles
        .Skip(currentPage * TILES_PER_PAGE)
        .Take(TILES_PER_PAGE);
```

## Exception Handling

### ✅ DO: Use meaningful exception messages

```csharp
public static TilesetViewModel Tileset =>
    tileset ?? throw new Exception("Tileset not initialized");
```

## Spacing and Layout

### ✅ DO: Use consistent spacing

- Empty line after class declaration
- Group related members together
- Space around operators and after commas

### ✅ DO: Align similar declarations when it improves readability

```csharp
Rectangle pageUpButton;
Rectangle pageDownButton;
Rectangle upArrowSource;
Rectangle downArrowSource;
```

## Type Usage

### ✅ DO: Use `var` when type is obvious, explicit types when clarity is needed

```csharp
var animatedSprite = new AnimatedSprite(); // Type is obvious
Rectangle destination = new(position.X, position.Y, width, height); // Type helps clarity
```

### ✅ DO: Use nullable reference types appropriately

```csharp
private static TilesetViewModel? tileset;
public static TilemapViewModel? Tilemap { get; set; }
```

## Summary of Key Rules

1. **NO underscore prefixes** on private variables
2. **DON'T declare variables in methods** for values that don't change
3. Use **camelCase** for private fields and local variables
4. Use **readonly** for fields that don't change after construction
5. Use **early returns** for cleaner control flow
6. Keep methods **focused and single-purpose**
7. Use **meaningful names** that express intent
