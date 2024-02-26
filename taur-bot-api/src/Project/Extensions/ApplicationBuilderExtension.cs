using System.Reflection;
using Microsoft.EntityFrameworkCore;
using taur_bot_api.Api.Services.ApiServices;
using taur_bot_api.Api.Services.Providers;
using taur_bot_api.Api.Services.Providers.Interfaces;
using taur_bot_api.Database;
using taur_bot_api.Database.Repositories;
using taur_bot_api.src.Api.Services.ApiServices;
using taur_bot_api.src.Database.Repositories;

namespace taur_bot_api.Project.Extensions;

public static class ApplicationBuilderExtension
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddControllers();
        
        builder.Services.AddDbContextFactory<AppDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(builder.Configuration["ConnectionStrings:string"]);
        });
        
        builder.Services.AddSingleton<ICryptoFactory, CryptoFactory>();
		var networks = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.GetInterfaces().Contains(typeof(ICryptoApiProvider)));

        foreach (var network in networks)
        {
            builder.Services.Add(new ServiceDescriptor(typeof(ICryptoApiProvider), network, ServiceLifetime.Singleton));
        }
    }
    
    public static void AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UserRepository>();
        builder.Services.AddScoped<InvestmentRepository>();
        builder.Services.AddScoped<ReferralNodeRepository>();
        builder.Services.AddScoped<WalletRepository>();
        builder.Services.AddScoped<OperationsRepository>();
        builder.Services.AddScoped<WithdrawRepository>();
    }
    
    public static void AddHttpClients(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient("CryptoApi",client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["EthApi"]!);
        });
    }
    
    public static void AddApiServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<InvestmentService>();
        builder.Services.AddScoped<AuthorizationService>();
        builder.Services.AddScoped<ReferralService>();
        builder.Services.AddScoped<WalletService>();
        builder.Services.AddScoped<OperationService>();
        builder.Services.AddScoped<WithdrawService>();
        builder.Services.AddMemoryCache();
		builder.Services.AddScoped<LocalizationService>();
        builder.Services.AddScoped<UserStateService>();
    }
}