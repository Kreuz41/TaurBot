using taur_bot.Bot.Context;
using taur_bot.Commands;
using taur_bot.Singletones;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace taur_bot.src.Commands.Realizations;
public class MainMenu(IReadOnlySet<string> contents, string key) : BaseCommand(contents, key)
{
	public override async Task Execute(BotRequestContext context, CancellationToken cnlToken)
	{
		var localeStorage = context.Scope.ServiceProvider.GetService<LocaleStorage>();

		var personalCabinet = localeStorage!.GetLocaleText(context.Locale, "PersonalCabinet");
		var structure = localeStorage.GetLocaleText(context.Locale, "Structure");
		var finances = localeStorage.GetLocaleText(context.Locale, "Finances");
		var referralProgram = localeStorage.GetLocaleText(context.Locale, "ReferralProgram");
		var information = localeStorage.GetLocaleText(context.Locale, "Information");
		var supportChat = localeStorage.GetLocaleText(context.Locale, "SupportChat");
		var changeLanguage = localeStorage.GetLocaleText(context.Locale, "ChangeLanguage");
		var chooseOption = localeStorage.GetLocaleText(context.Locale, "ChooseOption");

		var keyboard = new ReplyKeyboardMarkup(new[]
		{
			new KeyboardButton[] { personalCabinet!, structure!, finances! },
			new KeyboardButton[] { referralProgram!, information!, supportChat! },
			new KeyboardButton[] { changeLanguage! }
		})
		{
			ResizeKeyboard = true
		};

		await context.Client.SendTextMessageAsync(
			chatId: context.Id,
			text: chooseOption!,
			replyMarkup: keyboard,
			cancellationToken: cnlToken
		);
	}
}
