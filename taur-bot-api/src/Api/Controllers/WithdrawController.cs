using Microsoft.AspNetCore.Mvc;
using taur_bot_api.Api.ResponseDto.Responses;
using taur_bot_api.Database.Models;
using taur_bot_api.src.Api.RequestDto;
using taur_bot_api.src.Api.Services.ApiServices;

namespace taur_bot_api.src.Api.Controllers;

[ApiController]
[Route("api/withdraw")]
public class WithdrawController(WithdrawService service) : ControllerBase
{
	/// <summary>
	/// Create withdraw
	/// </summary>
	/// <remarks>
	/// Create new withdraw with current params
	/// </remarks>
	/// <param name="dto"></param>
	/// <returns>Nothing</returns>
	/// <response code="201"/>
	[HttpPost("create/{userTgId:long}")]
	[ProducesResponseType(201)]
	public async Task<IResult> Create(long userTgId, [FromBody] WithdrawCreateDto dto)
	{
		await service.CreateWithdraw(userTgId, dto.Sum);
		return Results.Created();
	}
}
