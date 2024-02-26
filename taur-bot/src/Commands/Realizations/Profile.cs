using taur_bot.Bot.Context;
using taur_bot.HttpClients.HttpClientFactories;
using taur_bot.Singletones;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace taur_bot.Commands.Realizations;

public class Profile(IReadOnlySet<string> contents, string key) : BaseCommand(contents, key)
{
    public override async Task Execute(BotRequestContext context, CancellationToken cnlToken)
    {
		var localeStorage = context.Scope.ServiceProvider.GetService<LocaleStorage>();
		var httpFactory = context.Scope.ServiceProvider.GetService<TaurApiFactory>();
		var response = await httpFactory!.GetUserProfile(context.Id);
		if (response != null)
		{
			var isActiveText = response.IsActive ? localeStorage!.GetLocaleText(context.Locale, "CabinetActive") : localeStorage!.GetLocaleText(context.Locale, "CabinetInactive");
			var cabinetId = localeStorage.GetLocaleText(context.Locale, "CabinetId");
			var balance = localeStorage.GetLocaleText(context.Locale, "Balance");
			var invested = localeStorage.GetLocaleText(context.Locale, "Invested");
			var activateCabinet = localeStorage.GetLocaleText(context.Locale, "ActivateCabinet");
			var referralLink = localeStorage.GetLocaleText(context.Locale, "ReferralLink");
			var referrer = localeStorage.GetLocaleText(context.Locale, "Referrer");
			var messageText = $"""
							   {cabinetId}: {response.Id}
							   {balance}: {response.Balance:0.00} USDT
							   {invested}: {response.InvestedBalance:0.00} USDT
							   {isActiveText}
							   {activateCabinet}
							   {referralLink}: ссылка
							   {referrer}: {response.Referrer}
							   """;
			var keyboard = new ReplyKeyboardMarkup(new[]
				{
				new [] { new KeyboardButton("English") },
				new [] { new KeyboardButton("Русский") },
			})
			{
				ResizeKeyboard = true
			};
				await context.Client.SendTextMessageAsync(context.Id, text: messageText, replyMarkup: keyboard, cancellationToken: cnlToken);
		}


	}
}