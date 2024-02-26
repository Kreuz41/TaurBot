using Microsoft.AspNetCore.DataProtection.KeyManagement;
using taur_bot.Bot.Context;
using taur_bot.HttpClients.HttpClientFactories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace taur_bot.Commands;

public interface ICommand
{
    public Task Execute(BotRequestContext context, CancellationToken cnlToken);
    public bool CanHandleMessage(string message);
    public bool ContanitsKey(string key);
}