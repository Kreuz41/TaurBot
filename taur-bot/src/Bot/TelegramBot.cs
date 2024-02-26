using taur_bot.Bot.Context;
using taur_bot.Bot.Settings;
using taur_bot.Commands;
using taur_bot.HttpClients.HttpClientFactories;
using taur_bot.Singletones;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace taur_bot.Bot;

public class TelegramBot(BotSettings token, CommandStorage storage, TaurApiFactory factory, IServiceProvider provider)
{
	private readonly TelegramBotClient _client = new(token.Token);

	public void Start()
	{
		var cts = new CancellationTokenSource();
		var receiverOptions = new ReceiverOptions
		{
			AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery],
			ThrowPendingUpdates = false
		};

		_client.StartReceiving(
			HandleUpdateAsync,
			HandleErrorAsync,
			receiverOptions,
			cancellationToken: cts.Token);
	}

	private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cnlToken)
	{
		var text = update.Type == UpdateType.Message?update.Message!.Text:update.CallbackQuery!.Data;
		if (text == null) return;
		var id = update.Message != null ? update.Message.Chat.Id : update.CallbackQuery!.Message!.Chat.Id;
		using var scope = provider.CreateScope();
		var localService = scope.ServiceProvider.GetService<TaurApiFactory>();
		var locale = await localService?.GetUserLocale(id)!;
		var state = await localService.GetUserState(id);
		var trimmedText = text;
		if (text.StartsWith('/')) trimmedText = text.TrimStart('/').Split(' ')[0];

		var context = new BotRequestContext
		{
			Client = client,
			Update = update,
			Scope = scope,
			Locale = locale,
			Id = id,
			Pack = trimmedText == "pack" ? Convert.ToInt32(text.TrimStart('/').Split(' ')[1]) : state
		};
		ICommand command;
		if (state != 0 && update.Type == UpdateType.Message)
			command = storage.Commands.GetByKey("createInvestment")!;
		else
			command = storage.Commands.TryGetCommand(trimmedText) ?? storage.Commands.GetByKey(trimmedText) ?? storage.Commands.GetByKey("menu")!;
		if (command != null) await command.Execute(context, cnlToken)!;
	}

	private static Task HandleErrorAsync(ITelegramBotClient client, Exception ex, CancellationToken cnlToken)
	{
		return Task.CompletedTask;
	}
}