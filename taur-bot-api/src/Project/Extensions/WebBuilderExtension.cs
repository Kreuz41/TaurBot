using Microsoft.EntityFrameworkCore;
using taur_bot_api.Database;

namespace taur_bot_api.Project.Extensions;

public static class WebBuilderExtension
{
    public static async Task<IApplicationBuilder> SetupDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var conn = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await conn.Database.MigrateAsync();
        
        return app;
    }
    
    public static async Task<IApplicationBuilder> SetupMiddlewares(this WebApplication app)
    {
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStaticFiles();
        app.UseAuthorization();
        app.MapControllers();
        
        return app;
    }
}