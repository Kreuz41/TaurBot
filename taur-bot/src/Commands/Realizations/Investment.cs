using taur_bot.Singletones;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using taur_bot.Commands;
using taur_bot.Bot.Context;
using Telegram.Bot.Types;

namespace taur_bot.src.Commands.Realizations
{
	public class Investment(IReadOnlySet<string> contents, string key) : BaseCommand(contents, key)
	{
		public async override Task Execute(BotRequestContext context, CancellationToken cnlToken)
		{
			var localeStorage = context.Scope.ServiceProvider.GetService<LocaleStorage>();
			var packageChoiceText = localeStorage!.GetLocaleText(context.Locale, "ChoosePack");

			var inlineKeyboard = new InlineKeyboardMarkup(new[]
			{
				InlineKeyboardButton.WithCallbackData(localeStorage.GetLocaleText(context.Locale, "Pack1")!, "/pack 1"),
				InlineKeyboardButton.WithCallbackData(localeStorage.GetLocaleText(context.Locale, "Pack2")!, "/pack 2"),
				InlineKeyboardButton.WithCallbackData(localeStorage.GetLocaleText(context.Locale, "Pack3")!, "/pack 3"),
				InlineKeyboardButton.WithCallbackData(localeStorage.GetLocaleText(context.Locale, "Pack4")!, "/pack 4"),
				InlineKeyboardButton.WithCallbackData(localeStorage.GetLocaleText(context.Locale, "Pack5")!, "/pack 5"),
				InlineKeyboardButton.WithCallbackData(localeStorage.GetLocaleText(context.Locale, "Pack6")!, "/pack 6"),
			});

			await context.Client.SendTextMessageAsync(
				chatId: context.Id,
				text: packageChoiceText!,
				replyMarkup: inlineKeyboard
			);
		}
	}
}
