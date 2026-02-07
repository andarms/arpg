using Arpg.Engine;
using Arpg.Engine.GameConsole;

namespace Arpg.Editor.GameConsole.Commands;


public class RoomsService
{
  private string[]? cachedRooms = null;

  public string[] GetAllRooms()
  {
    if (cachedRooms == null)
    {
      cachedRooms = LoadRoomsFromProject();
    }
    return cachedRooms;
  }

  public string? GetRoomByIndex(int index)
  {
    var rooms = GetAllRooms();
    if (index >= 1 && index <= rooms.Length)
    {
      return rooms[index - 1]; // Convert from 1-based to 0-based indexing
    }
    return null;
  }

  public string[] GetNumberedRoomsList()
  {
    var rooms = GetAllRooms();
    return [.. rooms.Select((room, index) => $"{index + 1}. {room}")];
  }

  private string[] LoadRoomsFromProject()
  {
    // Use the default assets path for the editor which points to Arpg.Game/Assets/
    string assetsBasePath = "Arpg.Game/Assets";
    string roomsPath = Path.Combine(assetsBasePath, "Rooms");

    // Find project root by navigating up from the bin folder to find .sln file
    string currentDir = AppContext.BaseDirectory;
    string? projectRoot = null;

    DirectoryInfo? dir = new DirectoryInfo(currentDir);
    while (dir != null)
    {
      if (dir.GetFiles("*.sln").Any())
      {
        projectRoot = dir.FullName;
        break;
      }
      dir = dir.Parent;
    }

    if (projectRoot == null)
    {
      return ["Could not find project root (no .sln file found)"];
    }

    string fullPath = Path.Combine(projectRoot, roomsPath);

    if (!Directory.Exists(fullPath))
    {
      return [$"Rooms directory not found: {fullPath}"];
    }

    var roomFiles = Directory.GetFiles(fullPath, "*.room", SearchOption.AllDirectories);

    // Return relative paths from the Rooms directory
    return [.. roomFiles.Select(file => Path.GetRelativePath(fullPath, file))];
  }

  public void ClearCache()
  {
    cachedRooms = null;
  }
}

public class ShowCommand : BaseCommand
{
  public ShowCommand()
      : base("show", "Show information about commands or editor state", "show <info>", "info")
  {
  }

  public override string[]? Execute(string[] args, ICommandContext context)
  {
    if (!ValidateArgCount(args, 1, context))
    {
      return null;
    }

    var roomsService = context.GetService<RoomsService>();
    if (roomsService == null)
    {
      context.OutputError("RoomsService is not available");
      return null;
    }

    var infoType = args[0].ToLower();

    switch (infoType)
    {
      case "rooms":
        return roomsService.GetNumberedRoomsList();

      default:
        context.OutputError($"Unknown info type: {infoType}. Available: rooms");
        return null;
    }
  }


}