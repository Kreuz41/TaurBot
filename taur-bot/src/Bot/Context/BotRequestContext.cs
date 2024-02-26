using taur_bot.src.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace taur_bot.Bot.Context;

public class BotRequestContext
{
    public ITelegramBotClient Client { get; init; } = null!;
    public Update Update { get; init; } = null!;
    public int Locale { get; init; } = 0;
    public IServiceScope Scope { get; init; } = null!;
    public SelectedPack State { get; set; }
    public long Id { get; init; }
	public int Pack { get; internal set; }
}