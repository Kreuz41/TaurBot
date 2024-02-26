using taur_bot.Bot.Context;
using taur_bot.HttpClients.HttpClientFactories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace taur_bot.Commands.Realizations;

public class SetLanguage(IReadOnlySet<string> contents, string key) : BaseCommand(contents, key)
{
    public override async Task Execute(BotRequestContext context, CancellationToken cnlToken)
    {
        if(context.Update == null) return;

		var keyboard = new ReplyKeyboardMarkup(new[]
			{
				new [] { new KeyboardButton("English") },
				new [] { new KeyboardButton("Русский") },
				new [] { new KeyboardButton("Español") },
				new [] { new KeyboardButton("Қазақ") },
				new [] { new KeyboardButton("Deutsch") },
				new [] { new KeyboardButton("中文") }
			})
		{
			ResizeKeyboard = true
		};
		const string message = "Choose your language / Выберите язык";
        await context.Client.SendTextMessageAsync(context.Id, message, 
            cancellationToken: cnlToken, replyMarkup: keyboard);
    }
}