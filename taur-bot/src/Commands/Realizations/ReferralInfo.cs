using taur_bot.Bot.Context;
using taur_bot.Commands;
using taur_bot.Enums;
using taur_bot.HttpClients.HttpClientFactories;
using taur_bot.Singletones;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace taur_bot.src.Commands.Realizations;

public class ReferralInfo(IReadOnlySet<string> contents, string key) : BaseCommand(contents, key)
{
	public override async Task Execute(BotRequestContext context, CancellationToken cnlToken)
	{
		var localeStorage = context.Scope.ServiceProvider.GetService<LocaleStorage>();
		var httpFactory = context.Scope.ServiceProvider.GetService<TaurApiFactory>();

		var totalReferralsText = localeStorage!.GetLocaleText(context.Locale, "TotalReferrals");
		var referralsBalanceText = localeStorage.GetLocaleText(context.Locale, "ReferralsBalance");
		var referralsTurnoverText = localeStorage.GetLocaleText(context.Locale, "ReferralsTurnover");
		var downloadListButtonText = localeStorage.GetLocaleText(context.Locale, "DownloadListButton");

		var refInfo = await httpFactory!.GetReferralInfo(context.Id);

		var message = $"""
					  {totalReferralsText}: {refInfo.Count}
					  {referralsBalanceText}: {refInfo.Balance:0.00} USDT
					  {referralsTurnoverText}: {refInfo.Turnover:0.00} USDT
					  """;

		var keyboard = new InlineKeyboardMarkup(new[]
			{
				InlineKeyboardButton.WithCallbackData(downloadListButtonText!, "downloadRefs"),
			});

		await context.Client.SendTextMessageAsync(
			chatId: context.Id,
			text: message!,
			replyMarkup: keyboard,
			cancellationToken: cnlToken
		);
	}
}
