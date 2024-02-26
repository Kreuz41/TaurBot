using taur_bot.src.Dto;

namespace taur_bot.HttpClients.HttpClientFactories;

public class TaurApiFactory(IHttpClientFactory httpClient)
{
    private readonly HttpClient _client = httpClient.CreateClient("TaurApi");
    
    public async Task<int> GetUserLocale(long userTgId)
    {
		using var response = await _client.GetAsync($"api/user/getLang/userTgId={userTgId}");
        if (!response.IsSuccessStatusCode) return 0;
        return await response.Content.ReadFromJsonAsync<int>();
    }
    public async Task<int> GetUserState(long userTgId)
    {
		using var response = await _client.GetAsync($"api/user/getState/userTgId={userTgId}");
		if (!response.IsSuccessStatusCode) return 0;
		return await response.Content.ReadFromJsonAsync<int>();
	}

    
    public async Task<bool> CreateUserAsync(long userTgId, string name, string? referrerCode, int local)
    {
        var body = new
        {
            TelegramId = userTgId,
            name,
            referrerCode,
            local
        };
		using var response = await _client.PostAsJsonAsync($"api/user/create", body);
        return response.IsSuccessStatusCode;
    }

    public async Task ChangeUserLocale(long userTgId, int newLocale)
    {
        await _client.PutAsync($"api/user/setLang/userTgId={userTgId}&lang={newLocale}", null);
    }
    public async Task SetUserState(long userTgId, int state)
    {
        await _client.PutAsync($"api/user/setState/userTgId={userTgId}&state={state}", null);
    }
    public async Task<UserResponseDto?> GetUserProfile(long userTgId)
    {
		using var response = await _client.GetAsync($"api/user/get/{userTgId}");
		if (!response.IsSuccessStatusCode) return null;
		var profile = await response.Content.ReadFromJsonAsync<UserResponseDto>();
        return profile ?? null;
	}
    public async Task<decimal> CheckUserBalance(long userTgId)
    {
		using var response = await _client.GetAsync($"api/user/checkBalance/userTgId={userTgId}");
		if (!response.IsSuccessStatusCode) return 0;
		var profile = await response.Content.ReadFromJsonAsync<decimal>();
        return profile;
	} 
    public async Task<decimal> GetUserBalance(long userTgId)
    {
		using var response = await _client.GetAsync($"api/user/getBalance/userTgId={userTgId}");
		if (!response.IsSuccessStatusCode) return 0;
		var profile = await response.Content.ReadFromJsonAsync<decimal>();
        return profile;
	}
    public async Task<string> GetUserDepositWallet(long userTgId)
    {
		using var response = await _client.GetAsync($"api/user/getDepositWallet/userTgId={userTgId}");
		if (!response.IsSuccessStatusCode) return string.Empty;
		var profile = await response.Content.ReadFromJsonAsync<string>();
		return profile ?? string.Empty;
	}
    public async Task<ReferralDto> GetReferralInfo(long userTgId)
    {
		using var response = await _client.GetAsync($"api/user/getRefInfo/userTgId={userTgId}");
		if (!response.IsSuccessStatusCode) return new ReferralDto();
		var profile = await response.Content.ReadFromJsonAsync<ReferralDto>() ?? new ReferralDto();
		return profile;
	}
    public async Task<bool> CreateInvestment(long userTgId, decimal dealSum, int type)
    {
        InvestmentCreateDto dto = new()
        {
            DealSum = dealSum,
            InvestmentType = type
        };
		using var response = await _client.PostAsJsonAsync($"api/investment/create/userTgId={userTgId}", dto);
		return response.IsSuccessStatusCode;
	}
}