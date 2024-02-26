using taur_bot_api.Database;
using taur_bot_api.Database.Models;
using taur_bot_api.Database.Repositories;

namespace taur_bot_api.Api.Services.ApiServices;

public class AuthorizationService(UserRepository repository)
{
    public async Task<bool> Auth(long userTgId) =>
        await repository.GetUserByTgId(userTgId) != null;
    public async Task<bool> Authorize(long userTgId) =>
        await repository.IsUserAdmin(userTgId);
}