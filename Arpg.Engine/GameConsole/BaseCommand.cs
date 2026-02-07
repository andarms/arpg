namespace Arpg.Engine.GameConsole;

public abstract class BaseCommand : ICommand
{
  protected BaseCommand(string name, string description, string usage = "", params string[] aliases)
  {
    Name = name;
    Description = description;
    Usage = string.IsNullOrEmpty(usage) ? name : usage;
    Aliases = aliases;
  }

  public string Name { get; }
  public string Description { get; }
  public string[] Aliases { get; }
  public string Usage { get; }

  public abstract string[]? Execute(string[] args, ICommandContext context);

  /// <summary>
  /// Helper method to validate argument count
  /// </summary>
  protected bool ValidateArgCount(string[] args, int expected, ICommandContext context)
  {
    if (args.Length != expected)
    {
      context.OutputError($"Usage: {Usage}");
      return false;
    }
    return true;
  }

  /// <summary>
  /// Helper method to validate minimum argument count
  /// </summary>
  protected bool ValidateMinArgCount(string[] args, int minimum, ICommandContext context)
  {
    if (args.Length < minimum)
    {
      context.OutputError($"Usage: {Usage}");
      return false;
    }
    return true;
  }
}