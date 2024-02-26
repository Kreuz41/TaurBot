using Microsoft.AspNetCore.Mvc;
using taur_bot_api.Api.RequestDto;
using taur_bot_api.Api.Services.ApiServices;

namespace taur_bot_api.Api.Controllers;

[ApiController]
[Route("api/investment")]
public class InvestmentController(InvestmentService service, AuthorizationService authorizationService) : ControllerBase
{
    [HttpPost("create/userTgId={userTgId:long}")]
    public async Task<IResult> Create(long userTgId, [FromBody] InvestmentCreateDto dto)
    {
        if (!await authorizationService.Auth(userTgId)) return Results.Unauthorized();
        
        var result = await service.CreateInvestment(dto, userTgId);
        
        return result.Message == "Ok" ? Results.Ok(result) : Results.BadRequest(result);
    }

    [HttpPut("accrual/userTgId={userTgId:long}")]
    public async Task<IResult> Accrual(long userTgId)
    {
        if (!await authorizationService.Authorize(userTgId)) 
            return Results.Forbid();    
        await service.AccrualAllInvestments();
        
        return Results.Ok();
    }
    [HttpPut("close/userTgId={userTgId:long}&investId={investId:long}")]
	public async Task<IResult> CloseInvest(long userTgId, long investId)
	{
		if (!await authorizationService.Auth(userTgId))
            return Results.Unauthorized();

        if(DateTime.Now.DayOfWeek != DayOfWeek.Saturday) return Results.Forbid();
		var res = await service.CloseInvestment(investId);

		return res? Results.Ok() : Results.BadRequest();
	}
}