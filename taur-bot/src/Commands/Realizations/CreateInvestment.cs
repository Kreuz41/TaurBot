using taur_bot.Bot.Context;
using taur_bot.Commands;
using taur_bot.HttpClients.HttpClientFactories;
using taur_bot.Singletones;
using Telegram.Bot;

namespace taur_bot.src.Commands.Realizations;

public class CreateInvestment(IReadOnlySet<string> contents, string key) : BaseCommand(contents, key)
{
	public async override Task Execute(BotRequestContext context, CancellationToken cnlToken)
	{
		var httpFactory = context.Scope.ServiceProvider.GetService<TaurApiFactory>();
		var localeStorage = context.Scope.ServiceProvider.GetService<LocaleStorage>();
		var res = int.TryParse(context.Update.Message!.Text, out var sum);
		if(!res)
		{
			var errorMessage = localeStorage!.GetLocaleText(context.Locale, "IncorrectIntSum");
			await context.Client.SendTextMessageAsync(context.Id, text: errorMessage!, cancellationToken: cnlToken);
		}
		res = await httpFactory!.CreateInvestment(context.Id, sum, context.Pack);
		if (!res)
		{
			var errorMessage = localeStorage!.GetLocaleText(context.Locale, "NotEnoughMoney");
			await context.Client.SendTextMessageAsync(context.Id, text: errorMessage!, cancellationToken: cnlToken);
		}
		else
		{
			var successMessage = localeStorage!.GetLocaleText(context.Locale, "InvestmentCreated");
			successMessage = string.Format(successMessage!, context.Pack, sum);
			await context.Client.SendTextMessageAsync(context.Id, text: successMessage!, cancellationToken: cnlToken);
		}
		await httpFactory!.SetUserState(context.Id, 0);
	}
}
