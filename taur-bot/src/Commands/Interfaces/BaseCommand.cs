using System.Net.Http.Headers;
using taur_bot.Bot.Context;
using taur_bot.HttpClients.HttpClientFactories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace taur_bot.Commands;

public abstract class BaseCommand(IReadOnlySet<string> contents, string key) : ICommand
{
    public string Key { get; private set; } = key;
    public abstract Task Execute(BotRequestContext context, CancellationToken cnlToken);
    
    public virtual bool CanHandleMessage(string message) => 
        contents.Contains(message);
    public virtual bool ContanitsKey(string key) => Key == key;
}