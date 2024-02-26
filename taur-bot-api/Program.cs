using taur_bot_api.Project.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();
builder.AddRepositories();
builder.AddApiServices();
builder.AddHttpClients();

var app = builder.Build();

await app.SetupMiddlewares();
await app.SetupDatabaseAsync();

app.Run();