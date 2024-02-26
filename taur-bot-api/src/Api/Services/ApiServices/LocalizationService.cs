using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using taur_bot_api.Database;
using taur_bot_api.Database.Repositories;


namespace taur_bot_api.src.Api.Services.ApiServices;
public class LocalizationService(UserRepository repository, IMemoryCache memoryCache)
{
	private readonly TimeSpan _cacheExpiration = TimeSpan.FromHours(1);

	public async Task LoadUsersToCache()
	{
		var users = await repository.GetAllUsers();

		foreach (var user in users)
			memoryCache.Set($"LanguageId_{user.Id}", user.LocalId, _cacheExpiration);
	}

	public async Task<int> GetUserLanguageIdAsync(long userId)
	{
		if (LoadUserLangFromCache(userId) == 0)
			await LoadUsersToCache();
		return LoadUserLangFromCache(userId);
	}
	public int LoadUserLangFromCache(long userId) => memoryCache.TryGetValue($"LanguageId_{userId}", out int languageId) ? languageId : 0;
	public async Task<bool> SetUserLanguage(long userId, int lang) 
	{
		var res = await repository.UpdateLanguage(userId, lang);
		if(!res) return false;
		await LoadUsersToCache();
		return true;
	}
}

