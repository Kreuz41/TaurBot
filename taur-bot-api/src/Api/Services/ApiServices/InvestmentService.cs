using taur_bot_api.Api.RequestDto;
using taur_bot_api.Api.ResponseDto.Responses;
using taur_bot_api.Database.Repositories;
using taur_bot_api.src.Api.Enums;
using taur_bot_api.src.Api.Services.ApiServices;

namespace taur_bot_api.Api.Services.ApiServices;

public class InvestmentService(InvestmentRepository repository, UserService userService, OperationService operationService, ReferralService referralService)
{
    public async Task<InvestmentCreateResponseDto> CreateInvestment(InvestmentCreateDto dto, long userTgId)
    {
        var response = new InvestmentCreateResponseDto();
        var transaction = await repository.CreateTransaction();

        var user = await userService.GetByTgId(userTgId);

        var investType = await repository.GetInvestTypeById(dto.InvestmentType);
        if (investType == null)
        {
            response.Message = "Invalid invest type";
            return response;
        }

        if (dto.DealSum < investType.MinValue)
        {
            response.Message = "Deal sum less than min invest type value";
            return response;
        }
        
        var res = await repository.CreateInvestment(dto, user!.Id);
        if (res != "Ok")
        {
            response.Message = res;
            return response;
        }

        res = await userService.Withdraw(dto.DealSum, user.Id);
        if (res != "Ok")
        {
            response.Message = res;
            return response;
        }

        if(!user.IsActive)
            await userService.ActivateProfile(user.Id);
        await userService.InvestOpen(user.Id, dto.DealSum);
        await operationService.CreateOperation(dto.DealSum, user.Id, OperationsType.Investment);

        await repository.CommitTransaction(transaction);

        return response;
    }

    public async Task AccrualAllInvestments()
    {  
        var investments = await repository.GetAllInvestments();
        foreach (var investment in investments)
        {
            if (!investment.HaveFirstAccrual && DateTime.UtcNow - investment.StartDate > TimeSpan.FromHours(24))
            {
                investment.HaveFirstAccrual = true;
                continue;
            }

            investment.TotalPercent += 1;
        }
    }
    public async Task<bool> CloseInvestment(long id)
    {
        var investment = await repository.GetInvestment(id);
        if (investment == null)
            return false;
        await repository.CloseInvestment(id);

        var sum = investment.TotalPercent * investment.DealSum + investment.DealSum;
		if (!await userService.TopUpUserBalance(investment.UserId, sum))
            return false;

        return await referralService.TopUpReferrersAsync(investment.UserId, sum);
    }
}