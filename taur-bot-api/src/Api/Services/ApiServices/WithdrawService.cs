using taur_bot_api.Api.Services.ApiServices;
using taur_bot_api.src.Database.Models;
using taur_bot_api.src.Database.Repositories;

namespace taur_bot_api.src.Api.Services.ApiServices
{
	public class WithdrawService(WithdrawRepository repository, UserService userService)
	{
		public async Task<bool> CreateWithdraw(long userTgId, decimal sum)
		{
			var user = await userService.GetByTgId(userTgId);
			if(user == null)  return false; 
			await repository.CreateWithdraw(user.Id, sum);
			return true;
		}
		public async Task<IEnumerable<Withdraw>> GetUserWithdraws(long userId) => await repository.GetWithdraws(userId);
	}
}
