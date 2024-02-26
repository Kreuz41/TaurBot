using Microsoft.EntityFrameworkCore;
using taur_bot_api.Api.RequestDto;
using taur_bot_api.Database.Models;

namespace taur_bot_api.Database.Repositories;

public class UserRepository(AppDbContext context) : Repository(context)
{
    private readonly AppDbContext _context = context;

    public async Task<User> CreateUser(UserCreateDto dto)
    {
        const string digits = "0123456789";
        const string ch = "abcdefghijklmnopqrstuvwxyz";

        var alphabet = digits + ch + ch.ToUpper();

        var referrer = dto.ReferrerCode != null ? await GetUserByReferralCode(dto.ReferrerCode) : null;
        
        var user = new User
        {
            LocalId = dto.Local,
            ReferrerId = referrer?.Id,
            Name = dto.Name,
            ReferralCode = new string(Random.Shared.GetItems(alphabet.AsSpan(), 10)),
            TelegramId = dto.TelegramId
        };

        var createdUser = _context.Users.Add(user);
        await SaveChangesAsync();
        return createdUser.Entity;
    }

    public async Task<User?> GetUserByTgId(long id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.TelegramId == id);
        return user;
    }


    public async Task<User?> GetUserByReferralCode(string code) => 
        await _context.Users.FirstOrDefaultAsync(u => u.ReferralCode == code);

    public async Task<User?> GetUserById(long id) =>
        await _context.Users.FindAsync(id);

    public async Task<string> Withdraw(decimal dtoDealSum, long userId)
    {
        var response = "Ok";
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            response = "Cannot find user";
            return response;
        }
        
        user.Balance -= dtoDealSum;
        
        return response;
    }

    public async Task ChangeIsActive(long userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if(user == null) return;
        
        user.IsActive = true;
        await SaveChangesAsync();
    }

    public async Task TopUpInvestBalance(long userId, decimal dtoDealSum)
    {
        var user = await _context.Users.FindAsync(userId);
        if(user == null) return;
        
        user.InvestedBalance += dtoDealSum;
        await SaveChangesAsync();
    }

    public async Task<bool> IsUserAdmin(long userTgId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.TelegramId == userTgId);
        return user!.IsAdmin;
    }
    public async Task<int> GetUserLang(long userTgId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u=> u.TelegramId == userTgId);
        if(user == null) return 0; 
        return user.LocalId;
    }
    public async Task<bool> TopUpBalance(long userId, decimal sum)
    {
		var user = await _context.Users.FindAsync(userId);
        if(user == null) return false;
        user.Balance += sum;
        return await SaveChangesAsync();
	}

    public async Task<bool> UpdateState(long userId, int state)
    {
        var user = await _context.Users.FindAsync(userId);
        if(user == null) return false;
        user.State = state;
		return await SaveChangesAsync();
    }

    public async Task<bool> UpdateLanguage(long userId, int lang)
    {
		var user = await _context.Users.FindAsync(userId);
		if (user == null) return false;
		user.LocalId = lang;
		return await SaveChangesAsync();
	}

	public async Task<IEnumerable<User>> GetAllUsers()
	{
		var users = await _context.Users.ToListAsync();
        return users;
	}

	public async Task<bool> SetWallet(long userId, string wallet)
	{
		var user = await _context.Users.FindAsync(userId);
		if (user == null) return false;
		user.WithdrawWallet = wallet;
		return await SaveChangesAsync();
	}
}