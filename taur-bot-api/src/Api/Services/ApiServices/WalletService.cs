using taur_bot_api.Api.Enums;
using taur_bot_api.Api.Services.Providers;
using taur_bot_api.Database.Models;
using taur_bot_api.Database.Repositories;

namespace taur_bot_api.Api.Services.ApiServices;

public class WalletService(WalletRepository repository, ICryptoFactory cryptoFactory)
{
    public async Task CreateWallets(long userId)
    {
        foreach (var network in cryptoFactory.Networks)
        {
            var wallet = await network.CreateWallet();
            if (wallet == null) return;

            await repository.CreateWallets(userId, wallet.Address, wallet.Private_Key, network.Type);
        }
    }
    public async Task<decimal> GetUserWalletBalance(long userId)
    {
        decimal balance = 0;
        foreach (var network in cryptoFactory.Networks)
        {
            var wallet = await repository.GetWallet(userId, network.Type);
            if (wallet == null) return 0;
            
            var balanceInNetwork = await network.GetWalletBalance(wallet.Address) ?? 0;
            if(balanceInNetwork == 0) 
                continue;

            var master = new Wallet(); //тут надо придумать как реализовать мастера
            if(!await network.TransferToken(wallet, master, balanceInNetwork))
                balanceInNetwork = 0;

            balance += balanceInNetwork;
        }
        
        return balance;
    }
    public async Task<string> GetWallet(long userId)
    {
        var wallet = await repository.GetWallet(userId, WalletType.Trc20);
		return wallet != null ? wallet.Address : string.Empty;
	}
}