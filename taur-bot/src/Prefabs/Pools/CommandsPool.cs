using taur_bot.Commands;
using taur_bot.Commands.Realizations;

namespace taur_bot.Prefabs;

public class CommandsPool
{
    private readonly List<ICommand> _commands = [];

    public void AddCommand(ICommand command)
    {
        _commands.Add(command);
    }

    public ICommand? TryGetCommand(string message)
    {
        return _commands.FirstOrDefault(command => command.CanHandleMessage(message));
    }
    public ICommand? GetByKey(string key) => _commands.FirstOrDefault(command => command.ContanitsKey(key));
}