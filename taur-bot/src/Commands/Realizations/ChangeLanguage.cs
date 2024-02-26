using taur_bot.Bot.Context;
using taur_bot.Enums;
using taur_bot.HttpClients.HttpClientFactories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace taur_bot.Commands.Realizations;

public class ChangeLanguage(IReadOnlySet<string> contents, string key) : BaseCommand(contents, key)
{
    public override async Task Execute(BotRequestContext context, CancellationToken cnlToken)
    {
		var httpFactory = context.Scope.ServiceProvider.GetService<TaurApiFactory>();
		var lang = GetLocaleFromString(context.Update.Message!.Text!);
		await httpFactory?.ChangeUserLocale(context.Id, (int)lang)!;
	}
	public Locale GetLocaleFromString(string localeName)
	{
		return localeName switch
		{
			"Русский" => Locale.Russian,
			"Español" => Locale.Spain,
			"Қазақ" => Locale.Kazakh,
			"Deutsch" => Locale.German,
			"中文" => Locale.Chinese,
			_ => Locale.English,
		};
	}
}