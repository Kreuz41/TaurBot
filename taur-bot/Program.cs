using taur_bot.Bot;
using taur_bot.Bot.Settings;
using taur_bot.HttpClients.HttpClientFactories;
using taur_bot.Singletones;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CommandStorage>();
builder.Services.AddSingleton<LocaleStorage>();
builder.Services.AddSingleton(builder.Configuration.GetSection("BotSettings").Get<BotSettings>());
builder.Services.AddSingleton<TelegramBot>();
builder.Services.AddSingleton<TaurApiFactory>();

builder.Services.AddHttpClient("TaurApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["TaurApi"]!);
});

var app = builder.Build();

var bot = app.Services.GetRequiredService<TelegramBot>();
bot.Start();

app.Run();