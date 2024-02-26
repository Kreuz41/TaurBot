using taur_bot.Bot.Context;
using taur_bot.Commands;
using taur_bot.Singletones;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace taur_bot.src.Commands.Realizations;

public class Finances(IReadOnlySet<string> contents, string key) : BaseCommand(contents, key)
{
	public override async Task Execute(BotRequestContext context, CancellationToken cnlToken)
	{
		var localeStorage = context.Scope.ServiceProvider.GetService<LocaleStorage>();
		var depositButton = localeStorage!.GetLocaleText(context.Locale, "Deposit");
		var investButton = localeStorage.GetLocaleText(context.Locale, "Invest");
		var withdrawButton = localeStorage.GetLocaleText(context.Locale, "Withdraw");
		var financesTitle = localeStorage.GetLocaleText(context.Locale, "FinancesTitle");

		var keyboard = new InlineKeyboardMarkup(new[]
		{ 
			InlineKeyboardButton.WithCallbackData(depositButton!, "deposit"), 
			InlineKeyboardButton.WithCallbackData(investButton!, "invest"),
			InlineKeyboardButton.WithCallbackData(withdrawButton!, "withdraw")
		});

		await context.Client.SendTextMessageAsync(
			chatId: context.Id,
			text: financesTitle!,
			replyMarkup: keyboard,
			cancellationToken: cnlToken
		);
	}
}
