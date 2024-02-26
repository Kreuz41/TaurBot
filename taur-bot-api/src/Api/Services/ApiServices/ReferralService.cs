using Microsoft.EntityFrameworkCore;
using taur_bot_api.Database;
using taur_bot_api.Database.Models;
using taur_bot_api.Database.Repositories;

namespace taur_bot_api.Api.Services.ApiServices;

public class ReferralService(ReferralNodeRepository repository, UserRepository userRepository)
{
	private readonly decimal[] _incomePercent = [0, 0.12m, 0.07m, 0.03m, 0.02m, 0.01m];
    public async Task CreateReferralTree(User user)
    {
        if(user.ReferrerId == null) return;
        const int linesInStructure = 5;
        var currentReferrer = user.Referrer;
        for (var i = 0; i < linesInStructure; i++)
        {
            await repository.CreateNode(currentReferrer!.Id, user.Id, i + 1);
            if(currentReferrer.ReferrerId == null) break;
            currentReferrer = await userRepository.GetUserById(currentReferrer.ReferrerId.Value);
        }
    }
    public async Task<List<ReferralNode>?> GetReferralTree(long userId, int offset) => await repository.GetTree(userId, offset);
	public async Task<List<ReferralNode>?> GetAllReferrals(long userId) => await repository.GetFullTree(userId);
	public async Task<bool> TopUpReferrersAsync(long userId, decimal amount)
	{
		var user = await userRepository.GetUserById(userId);
		if(user == null) return false;	
		await TopUpReferrersRecursive(user, amount);
		return true;
	}

	private async Task TopUpReferrersRecursive(User user, decimal amount)
	{
		if (user.ReferrerId == null)
			return;

		var referrer =  await userRepository.GetUserById(user.ReferrerId.Value);
		if (referrer != null)
		{
			var line = await repository.GetLine(referrer.Id, user.Id);
			decimal amountTopUp = amount * _incomePercent[line];
			await userRepository.TopUpBalance(referrer.Id, amountTopUp);
			await TopUpReferrersRecursive(referrer, amount);
		}
	}
}