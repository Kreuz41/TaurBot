using Microsoft.Extensions.Caching.Memory;
using taur_bot_api.Database.Repositories;
namespace taur_bot_api.src.Api.Services.ApiServices;


public class UserStateService(UserRepository repository, IMemoryCache memoryCache)
{
	private readonly TimeSpan _cacheExpiration = TimeSpan.FromHours(1);

	public async Task LoadUsersToCache()
	{
		var users = await repository.GetAllUsers();

		foreach (var user in users)
		{
			memoryCache.Set($"UserState_{user.Id}", user.State, _cacheExpiration);
		}
	}

	public async Task<int> GetUserStateAsync(long userId)
	{
		if (LoadUserStateFromCache(userId) == 0)
			await LoadUsersToCache();
		return LoadUserStateFromCache(userId);
	}

	private int LoadUserStateFromCache(long userId)
	{
		return memoryCache.TryGetValue($"UserState_{userId}", out int state) ? state : 0;
	}

	public async Task<bool> SetState(long userId, int newState)
	{
		var user = await repository.GetUserById(userId);
		if (user == null)
		{
			return false;
		}
		user.State = newState;
		var res = await repository.UpdateState(user.Id, newState);
		memoryCache.Set($"UserState_{userId}", newState, _cacheExpiration);
		return res;
	}
}
