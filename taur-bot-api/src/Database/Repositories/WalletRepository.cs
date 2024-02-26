using Microsoft.EntityFrameworkCore;
using taur_bot_api.Api.Enums;
using taur_bot_api.Api.Services.Providers;
using taur_bot_api.Database.Models;

namespace taur_bot_api.Database.Repositories;

public class WalletRepository(AppDbContext context) : Repository(context)
{
    public async Task CreateWallets(long userId, string address, string privateKey, WalletType type)
    {
        var wallet = new Wallet
        {
            UserId = userId,
            Address = address,
            PrivateKey = privateKey,
            NetworkType = type.ToString()
        };

        await context.Wallets.AddAsync(wallet);
        await SaveChangesAsync();
    }
    public async Task<Wallet> GetWallet(long userTgId, WalletType type)
    {
        var wallet = await context.Wallets.FirstOrDefaultAsync(w => w.UserId == userTgId && w.NetworkType == type.ToString());
        if (wallet == null) { return null; }
        return wallet;
	}
}