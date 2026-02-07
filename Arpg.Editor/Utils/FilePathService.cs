namespace Arpg.Editor.Utils;

public static class FilePathService
{
  private static string? projectRoot;

  public static string GetProjectRoot()
  {
    if (projectRoot != null) return projectRoot;

    string currentDir = AppContext.BaseDirectory;
    DirectoryInfo? dir = new(currentDir);
    while (dir != null)
    {
      if (dir.GetFiles("*.sln").Length != 0)
      {
        projectRoot = dir.FullName;
        return projectRoot;
      }
      dir = dir.Parent;
    }
    throw new DirectoryNotFoundException("Could not find project root (no .sln file found)");
  }

  public static string GetAssetsDirectory()
  {
    return Path.Combine(GetProjectRoot(), "Arpg.Game", "Assets");
  }

  public static string GetRoomsDirectory()
  {
    return Path.Combine(GetAssetsDirectory(), "Rooms");
  }

  public static string GetTexturesDirectory()
  {
    return Path.Combine(GetAssetsDirectory(), "Textures");
  }

  public static string GetFontsDirectory()
  {
    return Path.Combine(GetAssetsDirectory(), "Fonts");
  }

  public static string GetAssetPath(string relativePath)
  {
    return Path.Combine(GetAssetsDirectory(), relativePath);
  }

  public static string GetTexturePath(string fileName)
  {
    return Path.Combine(GetTexturesDirectory(), fileName);
  }

  public static string GetRoomPath(string roomFileName)
  {
    return Path.Combine(GetRoomsDirectory(), roomFileName);
  }
}