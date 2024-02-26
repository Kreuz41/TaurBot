using taur_bot.Bot.Context;
using taur_bot.Commands;
using taur_bot.HttpClients.HttpClientFactories;
using taur_bot.Singletones;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace taur_bot.src.Commands.Realizations;

public class Deposit(IReadOnlySet<string> contents, string key) : BaseCommand(contents, key)
{
	public override async Task Execute(BotRequestContext context, CancellationToken cnlToken)
	{
		var httpFactory = context.Scope.ServiceProvider.GetService<TaurApiFactory>();
		var localeStorage = context.Scope.ServiceProvider.GetService<LocaleStorage>();

		await httpFactory!.CheckUserBalance(context.Id);
		var balance = await httpFactory!.GetUserBalance(context.Id);
		var wallet = await httpFactory!.GetUserDepositWallet(context.Id);

		var balanceMessage = localeStorage!.GetLocaleText(context.Locale, "Balance") + balance;
		var depositMessage = localeStorage.GetLocaleText(context.Locale, "DepositAddress") + wallet;
		var commissionMessage = localeStorage.GetLocaleText(context.Locale, "ConsiderCommission");

		await context.Client.SendTextMessageAsync(context.Id, balanceMessage, cancellationToken: cnlToken);
		await context.Client.SendTextMessageAsync(context.Id, depositMessage, cancellationToken: cnlToken);

		var keyboard = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(localeStorage.GetLocaleText(context.Locale, "Deposit")!, "deposit"));
		await context.Client.SendTextMessageAsync(
			context.Id,
			commissionMessage!,
			replyMarkup: keyboard,
			cancellationToken: cnlToken
		);
	}
}
