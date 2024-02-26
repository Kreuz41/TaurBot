using Microsoft.EntityFrameworkCore.Storage;

namespace taur_bot_api.Database.Repositories;

public class Repository(AppDbContext context)
{
    protected async Task<bool> SaveChangesAsync()
    {
        var isSuccess = true;
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            isSuccess = false;
        }

        return isSuccess;
    }

    public async Task<IDbContextTransaction> CreateTransaction() =>
        await context.Database.BeginTransactionAsync();

    public async Task CommitTransaction(IDbContextTransaction transaction) =>
        await transaction.CommitAsync();
    
    public async Task RollbackTransaction(IDbContextTransaction transaction) =>
        await transaction.CommitAsync();
}