namespace Arpg.Engine.GameConsole;

public interface ICommand
{
  string Name { get; }

  string Description { get; }

  string[] Aliases { get; }

  string Usage { get; }

  string[]? Execute(string[] args, ICommandContext context);
}