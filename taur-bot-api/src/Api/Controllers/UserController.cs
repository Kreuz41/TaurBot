using Microsoft.AspNetCore.Mvc;
using taur_bot_api.Api.RequestDto;
using taur_bot_api.Api.ResponseDto;
using taur_bot_api.Api.ResponseDto.Responses;
using taur_bot_api.Api.Services.ApiServices;
using taur_bot_api.Database.Models;
using taur_bot_api.src.Api.Services.ApiServices;

namespace taur_bot_api.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(UserService service, AuthorizationService authorizationService, LocalizationService localizationService, UserStateService userStateService) : ControllerBase
{
    /// <summary>
    /// Registration user in system
    /// </summary>
    /// <remarks>
    /// Create new user with current params
    /// </remarks>
    /// <param name="dto"></param>
    /// <returns>Nothing</returns>
    /// <response code="201"/>
    [HttpPost("create")]
    [ProducesResponseType(201)]
    public async Task<IResult> Create([FromBody] UserCreateDto dto)
    {
        var res = await service.Create(dto);
        
        return res ? Results.Created() : Results.Conflict();
    }
    
    /// <summary>
    /// Return user profile data
    /// </summary>
    /// <remarks>
    /// Find user in database by id
    /// Find his referrer if he exist
    /// Build response object
    /// </remarks>
    /// <param name="userTgId"></param>
    /// <returns>User profile data</returns>
    /// <response code="200">User found. Returns profile</response>
    /// <response code="404">User not found</response>
    [ProducesResponseType(typeof(UserProfileDto), 200)]
    [ProducesResponseType(404)]
    [HttpGet("get/{userTgId:long}")]
    public async Task<IResult> GetByTelegramId(long userTgId)
    {
        var user = await service.GetByTgId(userTgId);
        
        return user == null ? Results.NotFound(userTgId) : Results.Ok(user);
    }

    /// <summary>
    /// Return user profile data
    /// </summary>
    /// <remarks>
    /// Find user's referral tree 
    /// Build response object
    /// </remarks>
    /// <param name="userTelegramId"></param>
    /// <param name="offset"></param>
    /// <returns>User profile data</returns>
    /// <response code="200">User found. Returns referral info</response>
    /// <response code="404">User not found</response>
    [ProducesResponseType(typeof(List<ReferralNode>), 200)]
    [ProducesResponseType(404)]
	[HttpGet("getStructure/userTgId={userTelegramId:long}&offset={offset:int}")]
	public async Task<IResult> GetStructure(long userTelegramId, int offset)
    {
        if (!await authorizationService.Auth(userTelegramId)) return Results.Unauthorized();
        var tree = await service.GetRefStructure(userTelegramId, offset);
        return tree == null ? Results.NotFound(userTelegramId) : Results.Ok(tree);
    }

	[ProducesResponseType(typeof(List<ReferralNode>), 200)]
    [ProducesResponseType(404)]
	[HttpGet("getRefInfo/userTgId={userTelegramId:long}")]
	public async Task<IResult> GetReferralInfo(long userTelegramId)
    {
        if (!await authorizationService.Auth(userTelegramId)) return Results.Unauthorized();
        var info = await service.GetReferralInfo(userTelegramId);
        return info.Count == 0 ? Results.NotFound(userTelegramId) : Results.Ok(info);
    }


	/// <summary>
	/// Return user profile data
	/// </summary>
	/// <remarks>
	/// Find user's language id
	/// Build response object
	/// </remarks>
	/// <param name="userTelegramId"></param>
	/// <returns>User profile data</returns>
	/// <response code="200">User found. Returns language id</response>
	/// <response code="404">User not found. Return 0</response>
	[ProducesResponseType(typeof(int), 200)]
	[ProducesResponseType(404)]
	[HttpGet("getLang/userTgId={userTelegramId:long}")]
	public async Task<IResult> GetUserLanguage(long userTelegramId)
	{
		if (!await authorizationService.Auth(userTelegramId)) return Results.Unauthorized();
        var user = await service.GetByTgId(userTelegramId);
        if(user == null) return Results.NotFound(0);
		var lang = await localizationService.GetUserLanguageIdAsync(user.Id);
		return lang == 0 ? Results.NotFound(lang) : Results.Ok(lang);
	}

	/// <summary>
	/// Return user profile data
	/// </summary>
	/// <remarks>
	/// Find user's language id
	/// Build response object
	/// </remarks>
	/// <param name="userTelegramId"></param>
	/// <param name="lang"></param>
	/// <returns>User profile data</returns>
	/// <response code="200">User found. Returns language id</response>
	/// <response code="404">User not found. Return 0</response>
	[ProducesResponseType(typeof(int), 200)]
	[ProducesResponseType(404)]
	[HttpPut("setLang/userTgId={userTelegramId:long}&lang={lang:int}")]
	public async Task<IResult> SetUserLanguage(long userTelegramId, int lang)
	{
		if (!await authorizationService.Auth(userTelegramId)) return Results.Unauthorized();
        var user = await service.GetByTgId(userTelegramId);
        if(user == null) return Results.NotFound();
        
		var res = await localizationService.SetUserLanguage(user.Id, lang);
		return res ? Results.NotFound() : Results.Ok();
	}

	/// <summary>
	/// Return user profile data
	/// </summary>
	/// <remarks>
	/// Find user's language id
	/// Build response object
	/// </remarks>
	/// <param name="userTelegramId"></param>
	/// <returns>User profile data</returns>
	/// <response code="200">User found. Returns language id</response>
	/// <response code="404">User not found. Return 0</response>
	[ProducesResponseType(typeof(int), 200)]
	[ProducesResponseType(404)]
	[HttpGet("getState/userTgId={userTelegramId:long}")]
	public async Task<IResult> GetUserState(long userTelegramId)
	{
		if (!await authorizationService.Auth(userTelegramId)) return Results.Unauthorized();
        var user = await service.GetByTgId(userTelegramId);
        if(user == null) return Results.NotFound(0);
		var lang = await userStateService.GetUserStateAsync(user.Id);
		return lang == 0 ? Results.NotFound(lang) : Results.Ok(lang);
	}	
	
	/// <summary>
	/// Return user profile data
	/// </summary>
	/// <remarks>
	/// Find user's language id
	/// Build response object
	/// </remarks>
	/// <param name="userTelegramId"></param>
	/// <returns>User profile data</returns>
	/// <response code="200">User found. Returns language id</response>
	/// <response code="404">User not found. Return 0</response>
	[ProducesResponseType(typeof(int), 200)]
	[ProducesResponseType(404)]
	[HttpPut("setState/userTgId={userTelegramId:long}&state={state:int}")]
	public async Task<IResult> UpdateUserState(long userTelegramId, int state)
	{
		if (!await authorizationService.Auth(userTelegramId)) return Results.Unauthorized();
        var user = await service.GetByTgId(userTelegramId);
        if(user == null) return Results.NotFound(0);
		var res = await userStateService.SetState(user.Id, state);
		return res ? Results.Ok() : Results.NotFound();
	}


    /// <summary>
	/// Return balance
	/// </summary>
	/// <remarks>
    /// Check user's wallet
	/// Return user's balance
	/// </remarks>
	/// <param name="userTelegramId"></param>
	/// <returns>User profile data</returns>
	/// <response code="200">User found. Returns balance on wallet</response>
	/// <response code="404">User not found. Return 0</response>
	[ProducesResponseType(typeof(int), 200)]
	[ProducesResponseType(404)]
	[HttpGet("getBalance/userTgId={userTelegramId:long}")]
	public async Task<IResult> GetUserBalance(long userTelegramId)
	{
		if (!await authorizationService.Auth(userTelegramId)) return Results.Unauthorized();
		var user = await service.GetByTgId(userTelegramId);
		return user == null ? Results.NotFound() : Results.Ok(user.Balance);
	}
	[ProducesResponseType(typeof(int), 200)]
	[ProducesResponseType(404)]
	[HttpGet("checkBalance/userTgId={userTelegramId:long}")]
	public async Task<IResult> CheckUserBalance(long userTelegramId)
	{
		if (!await authorizationService.Auth(userTelegramId)) return Results.Unauthorized();
		var balance = await service.CheckUserBalance(userTelegramId);
		return balance == 0 ? Results.NotFound() : Results.Ok(balance);
	}
	
	[ProducesResponseType(typeof(int), 200)]
	[ProducesResponseType(404)]
	[HttpGet("getOperations/userTgId={userTelegramId:long}&offset={offset:int}")]
	public async Task<IResult> GetUserOperations(long userTelegramId, int offset)
	{
		if (!await authorizationService.Auth(userTelegramId)) return Results.Unauthorized();
		var user = await service.GetByTgId(userTelegramId);
		if (user == null) return Results.NotFound(0);
		var operations = await service.GetOperations(user.Id, offset);
		return operations == null ? Results.NotFound() : Results.Ok(operations);
	}

	[ProducesResponseType(typeof(int), 200)]
	[ProducesResponseType(404)]
	[HttpPut("setWallet/userTgId={userTelegramId:long}&wallet={wallet}")]
	public async Task<IResult> SetWallet(long userTelegramId, string wallet)
	{
		if (!await authorizationService.Auth(userTelegramId)) return Results.Unauthorized();
		var user = await service.GetByTgId(userTelegramId);
		if (user == null) return Results.NotFound(0);
		var res = await service.SetWithdrawWallet(user.Id, wallet);
		return res ? Results.NotFound() : Results.Ok(res);
	}

	[ProducesResponseType(typeof(int), 200)]
	[ProducesResponseType(404)]
	[HttpGet("getDepositWallet/userTgId={userTelegramId:long}")]
	public async Task<IResult> SetWallet(long userTelegramId)
	{
		if (!await authorizationService.Auth(userTelegramId)) return Results.Unauthorized();
		var user = await service.GetByTgId(userTelegramId);
		if (user == null) return Results.NotFound(string.Empty);
		var res = await service.GetDepositWallet(user.Id);
		return res == string.Empty ? Results.NotFound(res) : Results.Ok(res);
	}
}