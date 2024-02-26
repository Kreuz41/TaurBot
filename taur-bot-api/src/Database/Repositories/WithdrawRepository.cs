using Microsoft.EntityFrameworkCore;
using taur_bot_api.Api.Enums;
using taur_bot_api.Database;
using taur_bot_api.Database.Models;
using taur_bot_api.Database.Repositories;
using taur_bot_api.src.Api.Enums;
using taur_bot_api.src.Database.Models;

namespace taur_bot_api.src.Database.Repositories;

public class WithdrawRepository(AppDbContext context) : Repository(context)
{
	public async Task CreateWithdraw(long userId, decimal sum, WalletType type = WalletType.Trc20)
	{
		var withdraw = new Withdraw
		{
			UserId = userId,
			Network = type.ToString(),
			Sum = sum,
		};
		await context.Withdraws.AddAsync(withdraw);
		await SaveChangesAsync();
	}
	public async Task<IEnumerable<Withdraw>> GetWithdraws(long userId, WalletType type = WalletType.Trc20)
	{
		var wallets = await context.Withdraws.Where(w => w.User.Id == userId && w.Status == WithdrawStatus.Waiting.ToString()).ToListAsync();
		if (wallets == null) return []; 
		return wallets;
	}

}
