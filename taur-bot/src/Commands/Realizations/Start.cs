using taur_bot.Bot.Context;
using taur_bot.HttpClients.HttpClientFactories;
using taur_bot.Singletones;
using taur_bot.src.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;

namespace taur_bot.Commands.Realizations;

public class Start(IReadOnlySet<string> contents, string key) : BaseCommand(contents, key)
{
    public override async Task Execute(BotRequestContext context, CancellationToken cnlToken)
    {
        var httpFactory = context.Scope.ServiceProvider.GetService<TaurApiFactory>();
        
        var message = context.Update;
        if(message == null) return;
        
        var userId = message.Message!.Chat.Id;
        var name = message.Message!.Chat.FirstName + ' ' + message.Message!.Chat.LastName;
        var code = message.Message.Text.Contains(' ') ? message.Message!.Text?.Split(' ')[1] : null;

        if (await httpFactory?.CreateUserAsync(userId, name, code, context.Locale)!)
        {
            
            var commandStorage = context.Scope.ServiceProvider.GetService<CommandStorage>();
            var command = commandStorage?.Commands.GetByKey("changeLanguage");
            command?.Execute(context, cnlToken);
        }

    }
}