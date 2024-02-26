using System.Text.Json;
using taur_bot.Commands;
using taur_bot.Commands.Realizations;
using taur_bot.Prefabs;
using taur_bot.src.Commands.Realizations;

namespace taur_bot.Singletones;

public class CommandStorage
{
    public CommandsPool Commands { get; } = new();
    
    public CommandStorage()
    {
        FillCommandPool();
    }

    private void FillCommandPool()
    {
        var json = File.ReadAllText("src/Prefabs/langs/commands.json");
        var commands = JsonSerializer.Deserialize<Dictionary<string, IList<string>>>(json);
        
        foreach (var (command, values) in commands!)
        {
            ICommand comm = command switch
            {
                "start" => new Start(values.ToHashSet(), command),
                "profile" => new Profile(values.ToHashSet(), command),
                "changeLanguage" => new SetLanguage(values.ToHashSet(), command),
                "language" => new ChangeLanguage(values.ToHashSet(), command),
                "menu" => new MainMenu(values.ToHashSet(), command),
                "finances"=> new Finances(values.ToHashSet(), command),
                "deposit" => new Deposit(values.ToHashSet(), command),
                "structure" => new ReferralInfo(values.ToHashSet(), command),
                "invest" => new Investment(values.ToHashSet(), command),
                "pack" => new PackSelected(values.ToHashSet(), command),
				"createInvestment" => new CreateInvestment(values.ToHashSet(), command),
				_ => new MainMenu(new HashSet<string>(), command)
            };
            Commands.AddCommand(comm);
        }
    }
}