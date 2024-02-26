using Microsoft.EntityFrameworkCore.Storage;
using taur_bot_api.Api.RequestDto;
using taur_bot_api.Api.ResponseDto;
using taur_bot_api.Api.ResponseDto.Responses;
using taur_bot_api.Api.Services.Providers.Interfaces;
using taur_bot_api.Database.Models;
using taur_bot_api.Database.Repositories;
using taur_bot_api.src.Api.Enums;
using taur_bot_api.src.Api.Responses.ResponseDto.Responses;
using taur_bot_api.src.Api.Services.ApiServices;
using taur_bot_api.src.Database.Models;

namespace taur_bot_api.Api.Services.ApiServices;

public class UserService(UserRepository repository, ReferralService referralService, WalletService walletService, OperationService operationService, LocalizationService localizationService)
{
    public async Task<bool> Create(UserCreateDto dto)
    {
        if (await GetByTgId(dto.TelegramId) != null) return false;
        var user = await repository.CreateUser(dto);

        await walletService.CreateWallets(user.Id);

        if (dto.ReferrerCode == null || await repository.GetUserByReferralCode(dto.ReferrerCode) == null) return false;
        await referralService.CreateReferralTree(user);
        return true;
    }

    public async Task<UserProfileDto?> GetByTgId(long id)
    {
        var user = await repository.GetUserByTgId(id);
        if (user == null) return null;

        var userProfile = new UserProfileDto(user);
        if (user.ReferrerId == null) return userProfile;

        var referrer = await repository.GetUserById(user.ReferrerId.Value);
        userProfile.Referrer = $"{referrer?.Name} {referrer?.Nickname}";

        return userProfile;
    }
    public async Task<ReferralDto> GetReferralInfo(long id)
    {
		var user = await repository.GetUserByTgId(id);
		if (user == null) return new ReferralDto();

		var tree = await referralService.GetAllReferrals(user.Id);
        if(tree==null) return new ReferralDto();
        var res = new ReferralDto
        {
            Count = tree.Count,
            Balance = tree.Sum(t => t.Referral.Balance),
            Turnover = tree.Sum(t => t.Referral.InvestedBalance)
        };
        return res;
    }

    private async Task<bool> IsMoneyEnough(long userId, decimal value)
    {
        var user = await repository.GetUserById(userId);
        return user?.Balance >= value;
    }
    public async Task<string> GetDepositWallet(long userId) => await walletService.GetWallet(userId);

    public async Task<string> Withdraw(decimal dtoDealSum, long userId)
    {
        if (!await IsMoneyEnough(userId, dtoDealSum)) return "Not enough money";
        return await repository.Withdraw(dtoDealSum, userId);
    }
    public async Task<List<ReferralNode>?> GetRefStructure(long userTgId, int offset)
    {
        var user = await repository.GetUserByTgId(userTgId);
        if (user == null) return null;
        var tree = await referralService.GetReferralTree(user.Id, offset);
        return tree;
    }

    public async Task ActivateProfile(long userId) =>
        await repository.ChangeIsActive(userId);

    public async Task InvestOpen(long userId, decimal dtoDealSum) =>
        await repository.TopUpInvestBalance(userId, dtoDealSum);

    public async Task<bool> IsAdmin(long userTgId)
    {
        if (await GetByTgId(userTgId) == null) return false;
        return await repository.IsUserAdmin(userTgId);
    }
    
    public async Task<int> GetUserLanguageId(long userTgId)
    {
        var user = await GetByTgId(userTgId);
		if (user == null) return 0;
        return await localizationService.GetUserLanguageIdAsync(user.Id);
	}

    
    public async Task<decimal> CheckUserBalance(long userTgId)
    {
		var user = await repository.GetUserByTgId(userTgId);
		if (user == null) return 0;
        
        var trans = await repository.CreateTransaction();
        
        var balance = await walletService.GetUserWalletBalance(user.Id);

        if (balance == 0.0m || !await repository.TopUpBalance(user.Id, balance))
        {
            await trans.RollbackAsync();
            return 0;
        }
        await operationService.CreateOperation(balance, user.Id, OperationsType.Deposit);


        await trans.CommitAsync();
        
        return balance;
	}
    public async Task<IEnumerable<Operation>> GetOperations(long userId, int offset) => await operationService.GetUserOperations(userId, offset);
    public async Task<bool> TopUpUserBalance(long userId, decimal sum) => await repository.TopUpBalance(userId, sum);
    public async Task<bool> SetWithdrawWallet(long userId, string wallet) => await repository.SetWallet(userId, wallet);
}