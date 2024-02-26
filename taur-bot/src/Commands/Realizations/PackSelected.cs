using taur_bot.Bot.Context;
using taur_bot.Commands;
using taur_bot.Enums;
using taur_bot.HttpClients.HttpClientFactories;
using taur_bot.Singletones;
using Telegram.Bot;

namespace taur_bot.src.Commands.Realizations;

public class PackSelected(IReadOnlySet<string> contents, string key) : BaseCommand(contents, key)
{
	public async override Task Execute(BotRequestContext context, CancellationToken cnlToken)
	{
		if (context.Pack == 0)
			return;
		var httpFactory = context.Scope.ServiceProvider.GetService<TaurApiFactory>();
		var localeStorage = context.Scope.ServiceProvider.GetService<LocaleStorage>();

		await httpFactory!.SetUserState(context.Id, context.Pack);
		var balance = await httpFactory!.GetUserBalance(context.Id);

		var message = localeStorage!.GetLocaleText(context.Locale, "AvailableForInvestment");
		message = string.Format(message!, balance);
		await context.Client.SendTextMessageAsync(context.Id, text: message, cancellationToken: cnlToken);
	}
}
